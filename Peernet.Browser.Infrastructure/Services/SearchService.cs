using Peernet.Browser.Application.Managers;
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

        public SearchService(ISettingsManager settingsManager)
        {
            searchClient = new SearchClient(settingsManager);
        }

        public async Task<SearchResultModel> Search(SearchFilterResultModel model)
        {
            var res = new SearchResultModel { Filters = model };
            var isNew = model.IsNewSearch;
            if (isNew)
            {
                var response = await searchClient.SubmitSearch(Map<SearchRequest>(model));
                if (response.Status != SearchRequestResponseStatusEnum.Success)
                {
                    return res;
                }
                model.Uuid = response.Id;
            }
            var reqMod = Map<SearchGetRequest>(model);
            var result = await searchClient.GetSearchResult(reqMod);
            var intervals = 0;
            while (isNew && result.IsEmpty && intervals < 20)
            {
                await Task.Delay(500);
                result = await searchClient.GetSearchResult(reqMod);
                intervals++;
            }

            res.Id = model.Uuid;
            res.StatusText = GetStatusText(result.Status);
            res.Stats = GetStats(result.Statistic);
            if (!result.Files.IsNullOrEmpty())
            {
                res.Rows = result.Files
                    .Select(x => new SearchResultRowModel(x))
                    .ToArray();
            }
            return res;
        }

        public async Task<string> Terminate(string id) => await searchClient.TerminateSearch(id);

        public IDictionary<FiltersType, int> GetEmptyStats() => GetStats(new SearchStatisticData());

        private string GetStatusText(SearchStatusEnum status)
        {
            switch (status)
            {
                case SearchStatusEnum.IdNotFound:
                    return "Search was terminated.";

                case SearchStatusEnum.KeepTrying:
                    return "Searching...";

                case SearchStatusEnum.NoMoreResults:
                    return "No results.";

                default:
                    return "";
            }
        }

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

        private SearchRequestSortTypeEnum Map(DataGridSortingNameEnum sortName, DataGridSortingTypeEnum sortType)
        {
            switch (sortName)
            {
                case DataGridSortingNameEnum.Name:
                    return sortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortNameAsc : SearchRequestSortTypeEnum.SortNameDesc;

                case DataGridSortingNameEnum.Date:
                    return sortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortDateAsc : SearchRequestSortTypeEnum.SortDateDesc;

                case DataGridSortingNameEnum.Size:
                    return sortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortSizeAsc : SearchRequestSortTypeEnum.SortSizeDesc;

                case DataGridSortingNameEnum.Share:
                    return sortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortSharedByCountAsc : SearchRequestSortTypeEnum.SortSharedByCountDesc;

                default:
                    return SearchRequestSortTypeEnum.SortNone;
            }
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
            if (searchGetRequest != null)
            {
                searchGetRequest.Limit = model.LimitOfResult;
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
            if (model.SortName != DataGridSortingNameEnum.None && model.SortType != DataGridSortingTypeEnum.None)
            {
                res.Sort = (int)Map(model.SortName, model.SortType);
            }
            if (model.FilterType != FiltersType.All)
            {
                res.FileType = (int)Map(model.FilterType);
            }
            if (!model.FileFormats.IsNullOrEmpty())
            {
                res.FileFormat = (int)model.FileFormats.First();
            }
            return res;
        }
    }
}