using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Models.Presentation.Home
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

        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
        private bool IsSizeDefault => SizeTo == SizeMax && SizeMin == SizeFrom;

        public DataGridSortingNameEnum SortName { get; set; }
        public DataGridSortingTypeEnum SortType { get; set; }

        public IEnumerable<FilterResultModel> Get()
        {
            var res = new List<FilterResultModel>();
            if (Time.HasValue)
            {
                res.Add(new FilterResultModel(Remove)
                {
                    Type = SearchFiltersType.TimePeriods,
                    Content = Time == TimePeriods.Custom ? $"{TimeFrom.Value.ToShortDateString()} - {TimeTo.Value.ToShortDateString()}" : Time.Value.GetDescription()
                });
            }
            if (!Healths.IsNullOrEmpty()) Healths.Foreach(x => res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.HealthType, Content = x.GetDescription() }));
            if (!FileFormats.IsNullOrEmpty()) FileFormats.Foreach(x => res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.FileFormats, Content = x.GetDescription() }));
            if (!IsSizeDefault) res.Add(new FilterResultModel(Remove) { Type = SearchFiltersType.Size, Content = $"{SizeFrom}MB - {SizeTo}MB" });
            return res;
        }

        public Tuple<DateTime, DateTime> GetDateRange()
        {
            var from = DateTime.Now;
            var to = DateTime.Now;
            switch (Time)
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

                case TimePeriods.Custom:
                    from = TimeFrom.Value;
                    to = TimeTo.Value;
                    break;
            }
            return new Tuple<DateTime, DateTime>(from, to);
        }

        private void Remove(FilterResultModel o) => OnRemoveAction?.Invoke(o.Type);
    }
}