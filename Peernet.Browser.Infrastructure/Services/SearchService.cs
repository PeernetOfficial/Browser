using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Domain.Search;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class SearchService : ISearchService
    {
        private readonly ISearchClient searchClient;

        public SearchService(ISearchClient searchClient)
        {
            this.searchClient = searchClient;
        }

        public IDictionary<FilterType, int> GetEmptyStats() => GetStats(new SearchStatisticData());

        public async Task<SearchResultModel> Search(SearchFilterResultModel searchFilterResultModel)
        {
            var searchResultModel = new SearchResultModel { Filters = searchFilterResultModel };
            var isNew = searchFilterResultModel.IsNewSearch;
            if (isNew)
            {
                var response = await searchClient.SubmitSearch(Map<SearchRequest>(searchFilterResultModel));
                if (response is not { Status: SearchRequestResponseStatusEnum.Success })
                {
                    return searchResultModel;
                }
                searchFilterResultModel.Uuid = response.Id;
            }
            var searchGetRequest = Map<SearchGetRequest>(searchFilterResultModel);
            if (searchFilterResultModel.ShouldReset)
            {
                searchGetRequest.Reset = 1;
            }

            var result = await searchClient.GetSearchResult(searchGetRequest);

            searchResultModel.Id = result.Status == SearchStatusEnum.IdNotFound ? string.Empty : searchFilterResultModel.Uuid;
            searchResultModel.Status = result.Status;
            searchResultModel.Stats = GetStats(result.Statistic);
            if (!result.Files.IsNullOrEmpty())
            {
                searchResultModel.Rows = result.Files.Select(f => new DownloadModel(f)).ToList();
            }

            return searchResultModel;
        }

        public async Task Terminate(string id)
        {
            if (id.IsNullOrEmpty()) return;
            try
            {
                await searchClient.TerminateSearch(id);
            }
            catch { }
        }

        private IDictionary<FilterType, int> GetStats(SearchStatisticData data)
        {
            if (data == null)
            {
                data = new SearchStatisticData();
            }

            return SearchResultModel
                .GetDefaultStats()
                .ToDictionary(x => x, y => y == FilterType.All ? data.Total : data.GetCount(Map(y)));
        }

        private SearchRequestSortTypeEnum Map(DataGridSortingNameEnum sortName, DataGridSortingTypeEnum sortType)
        {
            switch (sortName)
            {
                case DataGridSortingNameEnum.Name:
                    return sortType == DataGridSortingTypeEnum.Ascending ? SearchRequestSortTypeEnum.SortNameAsc : SearchRequestSortTypeEnum.SortNameDesc;

                case DataGridSortingNameEnum.Date:
                    return sortType == DataGridSortingTypeEnum.Ascending ? SearchRequestSortTypeEnum.SortDateAsc : SearchRequestSortTypeEnum.SortDateDesc;

                case DataGridSortingNameEnum.Size:
                    return sortType == DataGridSortingTypeEnum.Ascending ? SearchRequestSortTypeEnum.SortSizeAsc : SearchRequestSortTypeEnum.SortSizeDesc;

                case DataGridSortingNameEnum.SharedByCount:
                    return sortType == DataGridSortingTypeEnum.Ascending ? SearchRequestSortTypeEnum.SortSharedByCountAsc : SearchRequestSortTypeEnum.SortSharedByCountDesc;

                default:
                    return SearchRequestSortTypeEnum.SortNone;
            }
        }

        private LowLevelFileType Map(FilterType type)
        {
            switch (type)
            {
                case FilterType.Audio:
                    return LowLevelFileType.Audio;

                case FilterType.Video:
                    return LowLevelFileType.Video;

                case FilterType.Ebooks:
                    return LowLevelFileType.Ebook;

                case FilterType.Documents:
                    return LowLevelFileType.Document;

                case FilterType.Pictures:
                    return LowLevelFileType.Picture;

                case FilterType.Text:
                    return LowLevelFileType.Text;

                case FilterType.Binary:
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
            if (searchGetRequest != null)
            {
                searchGetRequest.Limit = model.Limit;
                searchGetRequest.Offset = model.Offset;
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
                var format = "yyyy-MM-dd HH:mm:ss";
                var range = model.GetDateRange();
                if (searchRequest != null)
                {
                    searchRequest.DateFrom = range.from.ToString(format);
                    searchRequest.DateTo = range.to.ToString(format);
                }
                if (searchGetRequest != null)
                {
                    searchGetRequest.From = range.from.ToString(format);
                    searchGetRequest.To = range.to.ToString(format);
                }
            }
            if (model.SizeFrom.HasValue)
            {
                res.SizeMin = model.SizeFrom.Value;
            }
            if (model.SizeTo.HasValue)
            {
                res.SizeMax = model.SizeTo.Value;
            }
            if (model.AdvancedFilter.SortName != DataGridSortingNameEnum.None && model.AdvancedFilter.SortType != DataGridSortingTypeEnum.None)
            {
                res.Sort = (int)Map(model.AdvancedFilter.SortName, model.AdvancedFilter.SortType);
            }
            if (model.FilterType != FilterType.All)
            {
                res.FileType = (int)Map(model.FilterType);
            }
            if (model.FileFormat != FileFormat.None)
            {
                res.FileFormat = (int)model.FileFormat;
            }

            return res;
        }
    }
}