using Peernet.Browser.Application.Enums;
using System;
using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class SearchFilterResultModel
    {
        public FileFormats? FileFormat { get; set; }
        public FiltersType FilterType { get; set; }
        public HealthType? Health { get; set; }
        public string InputText { get; set; }
        public Action<SearchFiltersType> OnRemoveAction { get; set; }
        public SortOrders? Order { get; set; }
        public string PrevId { get; set; }
        public int SizeFrom { get; set; }
        public int SizeMax { get; set; }
        public int SizeMin { get; set; }
        public int SizeTo { get; set; }
        public TimePeriods? Time { get; set; }
        private bool IsSizeDefault => SizeTo == SizeMax && SizeMin == SizeFrom;

        public IEnumerable<FilterResultModel> Get()
        {
            var res = new List<FilterResultModel>();
            if (Time.HasValue) res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.TimePeriods, Content = Time.Value.GetDescription() });
            if (Health.HasValue) res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.HealthType, Content = Health.Value.GetDescription() });
            if (FileFormat.HasValue) res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.FileFormats, Content = FileFormat.Value.GetDescription() });
            if (Order.HasValue) res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.Sortorder, Content = Order.Value.GetDescription() });
            if (!IsSizeDefault) res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.Size, Content = $"{SizeFrom}MB - {SizeTo}MB" });
            return res;
        }

        private void Remove(FilterResultModel o) => OnRemoveAction?.Invoke(o.Type);
    }
}