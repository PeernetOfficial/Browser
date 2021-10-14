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

        private readonly IDictionary<string, SearchResult> results = new Dictionary<string, SearchResult>();

        public SearchService(ISettingsManager settingsManager)
        {
            searchClient = new SearchClient(settingsManager);
        }

        public async Task<SearchResultModel> Search(SearchFilterResultModel model)
        {
            var res = new SearchResultModel { Filters = model, Size = new Tuple<int, int>(0, 15) };
            var response = await searchClient.SubmitSearch(Map(model));
            if (response.Status != SearchRequestResponseStatusEnum.Success)
            {
                return res;
            }
            res.Id = response.Id;
            var result = await searchClient.GetSearchResult(response.Id);
            results.Add(response.Id, result);
            res.Stats = GetStats(result.Statistic);
            res.Rows = result.Files
                .Select(x => new SearchResultRowModel(x))
                .ToArray();
            return res;
        }

        public async Task Terminate(string id)
        {
            await searchClient.TerminateSearch(id);
            results.Remove(id);
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

        private SearchRequest Map(SearchFilterResultModel model)
        {
            var res = new SearchRequest
            {
                Term = model.InputText,
                Timeout = 0,
                MaxResults = 0,
                FileFormat = HighLevelFileType.NotUsed,
                FileType = LowLevelFileType.NotUsed,
                Sort = SearchRequestSortTypeEnum.SortNone,
                SizeMax = -1,
                SizeMin = -1
            };
            if (model.PrevId.IsNullOrEmpty())
            {
                res.Terminate = new[] { model.PrevId };
            }
            if (model.Time.HasValue)
            {
                var r = model.GetDateRange();
                res.DateFrom = r.from.ToString();
                res.DateTo = r.to.ToString();
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
            if (!model.Healths.IsNullOrEmpty())
            {
                //TODO: ??
            }
            if (!model.FileFormats.IsNullOrEmpty())
            {
                //TODO: res.FileFormat = model.FileFormat.Value;
            }
            return res;
        }
    }
}