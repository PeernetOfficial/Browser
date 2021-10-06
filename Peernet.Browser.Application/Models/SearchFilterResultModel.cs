using Peernet.Browser.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class SearchFilterResultModel
    {
        public FileFormats[] FileFormats { get; set; }
        public FiltersType FilterType { get; set; }
        public HealthType[] Healths { get; set; }
        public string InputText { get; set; }
        public Action<SearchFiltersType> OnRemoveAction { get; set; }
        public string PrevId { get; set; }
        public int SizeFrom { get; set; }
        public int SizeMax { get; set; }
        public int SizeMin { get; set; }
        public int SizeTo { get; set; }
        public TimePeriods? Time { get; set; }
        private bool IsSizeDefault => SizeTo == SizeMax && SizeMin == SizeFrom;

        public DataGridSortingNameEnum SortName { get; set; }
        public DataGridSortingTypeEnum SortType { get; set; }

        public IEnumerable<FilterResultModel> Get()
        {
            var res = new List<FilterResultModel>();
            if (Time.HasValue) res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.TimePeriods, Content = Time.Value.GetDescription() });
            if (!Healths.IsNullOrEmpty()) Healths.Foreach(x => res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.HealthType, Content = x.GetDescription() }));
            if (!FileFormats.IsNullOrEmpty()) FileFormats.Foreach(x => res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.FileFormats, Content = x.GetDescription() }));
            if (!IsSizeDefault) res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.Size, Content = $"{SizeFrom}MB - {SizeTo}MB" });
            return res;
        }

        private void Remove(FilterResultModel o) => OnRemoveAction?.Invoke(o.Type);
    }
}