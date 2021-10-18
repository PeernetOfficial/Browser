using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    public class CalendarRangeExtension : Calendar
    {
        protected override void OnSelectedDatesChanged(SelectionChangedEventArgs e)
        {
            var selectedDates = SelectedDates
                .OrderBy(x => x)
                .ToArray();

            SelectedDate = selectedDates.FirstOrDefault();
            SelectedDateTo = selectedDates.LastOrDefault();

            if (selectedDates.Length == 1 && SelectedDateTo.HasValue && SelectedDate.HasValue && SelectedDateTo != SelectedDate)
            {
                SelectedDates.AddRange(SelectedDate.Value, SelectedDateTo.Value);
            }
        }

        public static readonly DependencyProperty SelectedDateToProperty = DependencyProperty.Register("SelectedDateTo", typeof(DateTime?), typeof(CalendarRangeExtension));
        public static readonly DependencyProperty SelectedDateToOnlySetProperty = DependencyProperty.Register("SelectedDateToOnlySet", typeof(DateTime?), typeof(CalendarRangeExtension), new PropertyMetadata(null, OnChange));

        public DateTime? SelectedDateTo
        {
            get => (DateTime?)GetValue(SelectedDateToProperty);
            set => SetValue(SelectedDateToProperty, value);
        }

        public DateTime? SelectedDateToOnlySet
        {
            get => (DateTime?)GetValue(SelectedDateToOnlySetProperty);
            set => SetValue(SelectedDateToOnlySetProperty, value);
        }

        private static void OnChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = (CalendarRangeExtension)d;
            if (e.NewValue == null || e.OldValue == null) return;
            var newValue = (DateTime)e.NewValue;
            o.SelectedDates.AddRange(o.SelectedDate.Value, o.SelectedDateTo.Value);
        }
    }
}