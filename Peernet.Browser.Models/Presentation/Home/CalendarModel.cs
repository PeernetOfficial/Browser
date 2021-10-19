using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class CalendarModel : MvxNotifyPropertyChanged
    {
        private DateTime? dateFrom;
        private DateTime? dateTo;

        public DateTime? DateFrom
        {
            get => dateFrom;
            set => SetProperty(ref dateFrom, value);
        }

        public DateTime? DateTo
        {
            get => dateTo;
            set => SetProperty(ref dateTo, value);
        }

        public void Rest() => Set(null, null);

        public void Set(DateTime? from, DateTime? to)
        {
            var wasChanged = from != DateFrom || to != DateTo;
            DateFrom = from;
            DateTo = to;
            if (wasChanged) OnSet?.Invoke();
        }

        public bool IsFill => DateFrom.HasValue && dateTo.HasValue;
        public Action OnSet { get; set; }
    }
}