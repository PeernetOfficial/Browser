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
            if (!model.PrevId.IsNullOrEmpty())
            {
                Terminate(model.PrevId);
            }

            var res = new SearchResultModel { Filters = model, Stats = GetStats(), Size = new Tuple<int, int>(0, 15) };
            var response = await searchClient.SubmitSearch(Map(model));
            if (response.Status != 0) return res;
            res.Id = response.Id;
            var result = await searchClient.GetSearchResult(response.Id);
            results.Add(response.Id, result);
            if (result.Status > 1) return res;
            res.Rows = result.Files
                .Select(x => new SearchResultRowModel(x))
                .ToArray();
            return res;
        }

        private async void Terminate(string id)
        {
            await searchClient.TerminateSearch(id);
            results.Remove(id);
        }

        private IDictionary<FiltersType, int> GetStats()
        {
            var res = new Dictionary<FiltersType, int>();
            res.Add(FiltersType.All, 2357);
            res.Add(FiltersType.Audio, 217);
            res.Add(FiltersType.Video, 844);
            res.Add(FiltersType.Ebooks, 629);
            res.Add(FiltersType.Documents, 632);
            res.Add(FiltersType.Pictures, 214);
            res.Add(FiltersType.Text, 182);
            res.Add(FiltersType.Binary, 1);
            return res;
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
                Sort = SearchRequestSortTypeEnum.SortNone
            };
            if (model.Time.HasValue)
            {
                var r = model.GetDateRange();
                res.DateFrom = r.Item1.ToString();
                res.DateTo = r.Item2.ToString();
            }
            if (!model.Healths.IsNullOrEmpty())
            {
                //TODO: ??
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
                        break;

                    case DataGridSortingNameEnum.Share:
                        res.Sort = model.SortType == DataGridSortingTypeEnum.Asc ? SearchRequestSortTypeEnum.SortRelevanceAsc : SearchRequestSortTypeEnum.SortRelevanceDec;
                        break;
                }
            }
            if (!model.FileFormats.IsNullOrEmpty())
            {
                //TODO: res.FileFormat = model.FileFormat.Value;
            }
            return res;
        }
    }
}