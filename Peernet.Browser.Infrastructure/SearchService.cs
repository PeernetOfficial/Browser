using Peernet.Browser.Application.Enums;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Infrastructure
{
    public class SearchService : ISearchService
    {
        private readonly ICmdClient api;

        private readonly IDictionary<string, SearchResult> results = new Dictionary<string, SearchResult>();

        public SearchService(ICmdClient api)
        {
            this.api = api;
        }

        public SearchResultModel Search(SearchFilterResultModel model)
        {
            if (!model.PrevId.IsNullOrEmpty()) Terminate(model.PrevId);
            var res = new SearchResultModel { Filters = model, Stats = GetStats(), Size = new Tuple<int, int>(0, 15) };
            var response = api.SubmitSearch(Map(model));
            if (response.Status != 0) return res;
            res.Id = response.Id;
            var result = api.ResturnSearch(response.Id);
            results.Add(response.Id, result);
            if (result.Status > 1) return res;
            res.Rows = result.Files
                .Select(x => new SearchResultRowModel(x))
                .ToArray();
            return res;
        }

        public void Terminate(string id)
        {
            api.TerminateSearch(id);
            results.Remove(id);
        }

        private Tuple<DateTime, DateTime> GetDateRange(TimePeriods p)
        {
            var from = DateTime.Now;
            switch (p)
            {
                case TimePeriods.Last24:
                    from = from.AddDays(-1);
                    break;

                case TimePeriods.LastWeek:
                    from = from.AddDays(-7);
                    break;

                case TimePeriods.LastMounth:
                    from = from.AddDays(-30);
                    break;

                case TimePeriods.LastYear:
                    from = from.AddDays(-365);
                    break;
            }
            return new Tuple<DateTime, DateTime>(from, DateTime.Now);
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
                var r = GetDateRange(model.Time.Value);
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