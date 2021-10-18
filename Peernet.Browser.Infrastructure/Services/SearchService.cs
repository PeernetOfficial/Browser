﻿using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Search;
using Peernet.Browser.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchClient searchClient;

        private readonly IDictionary<string, SearchResult> results = new Dictionary<string, SearchResult>();

        public SearchService(ISettingsManager settingsManager)
        {
            searchClient = new SearchClient(settingsManager);
        }

        public async Task<SearchResultModel> Search(SearchFilterResultModel model)
        {
            var res = new SearchResultModel
            {
                Filters = model,
                Size = new Tuple<int, int>(0, 15)
            };
            //Initialize search - POST
            if (model.IsNewSearch)
            {
                var response = await searchClient.SubmitSearch(Map<SearchRequest>(model));
                if (response.Status != SearchRequestResponseStatusEnum.Success)
                {
                    return res;
                }
                model.Uuid = response.Id;
                results.Add(response.Id, null);
            }
            //Make GET of result
            var result = await searchClient.GetSearchResult(Map<SearchGetRequest>(model));
            results[model.Uuid] = result;
            res.Id = model.Uuid;
            //Fill res (return) object
            res.Stats = GetStats(result.Statistic);
            res.Rows = result.Files
                .Select(x => new SearchResultRowModel(x))
                .ToArray();
            return res;
        }

        public async Task<string> Terminate(string id)
        {
            results.Remove(id);
            return await searchClient.TerminateSearch(id);
        }

        public IDictionary<FiltersType, int> GetEmptyStats() => GetStats(new SearchStatisticData());

        private IDictionary<FiltersType, int> GetStats(SearchStatisticData data)
        {
            if (data == null)
            {
                data = new SearchStatisticData();
            }

            return SearchResultModel
                .GetDefaultStats()
                .ToDictionary(x => x, y => y == FiltersType.All ? data.Total : data.GetCount(Map(y)));
        }

        private LowLevelFileType Map(FiltersType type)
        {
            switch (type)
            {
                case FiltersType.Audio:
                    return LowLevelFileType.Audio;

                case FiltersType.Video:
                    return LowLevelFileType.Video;

                case FiltersType.Ebooks:
                    return LowLevelFileType.Ebook;

                case FiltersType.Documents:
                    return LowLevelFileType.Document;

                case FiltersType.Pictures:
                    return LowLevelFileType.Picture;

                case FiltersType.Text:
                    return LowLevelFileType.Text;

                case FiltersType.Binary:
                    return LowLevelFileType.Binary;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Tempolary map function - in futher good to change with AutoMapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        private T Map<T>(SearchFilterResultModel model) where T : SearchRequestBase, new()
        {
            var res = new T();
            var searchRequest = res as SearchRequest;
            var searchGetRequest = res as SearchGetRequest;
            if (searchRequest != null)
            {
                searchRequest.Term = model.InputText;
            }
            if (model.Uuid.IsNullOrEmpty() && searchRequest != null)
            {
                searchRequest.Terminate = new[] { model.Uuid };
            }
            if (!model.Uuid.IsNullOrEmpty() && searchGetRequest != null)
            {
                searchGetRequest.Id = model.Uuid;
            }
            if (model.Time.HasValue)
            {
                var range = model.GetDateRange();
                if (searchRequest != null)
                {
                    searchRequest.DateFrom = range.from.ToString();
                    searchRequest.DateTo = range.to.ToString();
                }
                if (searchGetRequest != null)
                {
                    searchGetRequest.From = range.from.ToString();
                    searchGetRequest.To = range.to.ToString();
                }
            }
            if (model.SizeFrom.HasValue)
            {
                //res.SizeMin = model.SizeFrom.Value;
            }
            if (model.SizeTo.HasValue)
            {
                //res.SizeMax = model.SizeTo.Value;
            }
            if (model.SortName != DataGridSortingNameEnum.None && model.SortType != DataGridSortingTypeEnum.None)
            {
                switch (model.SortName)
                {
                    case DataGridSortingNameEnum.Name:
                        res.Sort = model.SortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortNameAsc : SearchRequestSortTypeEnum.SortNameDesc;
                        break;

                    case DataGridSortingNameEnum.Date:
                        res.Sort = model.SortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortDateAsc : SearchRequestSortTypeEnum.SortDateDesc;
                        break;

                    case DataGridSortingNameEnum.Size:
                        res.Sort = model.SortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortSizeAsc : SearchRequestSortTypeEnum.SortSizeDesc;
                        break;

                    case DataGridSortingNameEnum.Share:
                        res.Sort = model.SortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortSharedByCountAsc : SearchRequestSortTypeEnum.SortSharedByCountDesc;
                        break;
                }
            }
            if (model.FilterType != FiltersType.All)
            {
                res.FileType = Map(model.FilterType);
            }
            if (!model.Healths.IsNullOrEmpty())
            {
                //TODO: ??
            }
            if (!model.FileFormats.IsNullOrEmpty())
            {
                res.FileFormat = (HighLevelFileType)(int)model.FileFormats.First();
            }
            return res;
        }
    }
}