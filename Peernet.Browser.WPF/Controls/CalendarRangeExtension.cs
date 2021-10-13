using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    public class CalendarRangeExtension : Calendar
    {
        public CalendarRangeExtension() : base()
        {
            SelectedDatesChanged += CalendarRangeExtension_SelectedDatesChanged;
        }

        private void CalendarRangeExtension_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var count = SelectedDates.Count();
            if (count == 0)
            {
                SelectedDateTo = null;
            }
            if (count > 1)
            {
                SelectedDateTo = SelectedDates.Last();
            }
            if (count == 1 && SelectedDateTo.HasValue && SelectedDate.HasValue && SelectedDateTo != SelectedDate)
            {
                SelectedDates.AddRange(SelectedDate.Value, SelectedDateTo.Value);
            }
        }

        public static readonly DependencyProperty SelectedDateToProperty = DependencyProperty.Register("SelectedDateTo", typeof(DateTime?), typeof(CalendarRangeExtension));

        public DateTime? SelectedDateTo
        {
            get => (DateTime?)GetValue(SelectedDateToProperty);
            set => SetValue(SelectedDateToProperty, value);
        }
    }
}