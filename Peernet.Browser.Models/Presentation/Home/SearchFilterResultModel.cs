using System;
using System.Linq;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class SearchFilterResultModel
    {
        public bool IsNewSearch => Uuid.IsNullOrEmpty();
        public FileFormats[] FileFormats { get; set; }
        public FiltersType FilterType { get; set; }
        public string InputText { get; set; }
        public string Uuid { get; set; }
        public int? SizeFrom { get; set; }
        public int? SizeTo { get; set; }
        public TimePeriods? Time { get; set; }

        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }

        public int LimitOfResult { get; set; }

        public DataGridSortingNameEnum SortName { get; set; }
        public DataGridSortingTypeEnum SortType { get; set; }

        public bool IsCustomTimeFill => Time == TimePeriods.Custom;

        public (DateTime from, DateTime to) GetDateRange()
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

                case TimePeriods.LastMonth:
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
            return new(from, to);
        }
    }
}