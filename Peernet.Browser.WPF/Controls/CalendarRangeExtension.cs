using Peernet.Browser.Models.Presentation.Home;
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

            CustomDates.Set(selectedDates.FirstOrDefault(), selectedDates.LastOrDefault());
        }

        public static readonly DependencyProperty CustomDatesProperty = DependencyProperty.Register("CustomDates", typeof(CalendarModel), typeof(CalendarRangeExtension), new PropertyMetadata(null, OnChangeDates));

        public CalendarModel CustomDates
        {
            get => (CalendarModel)GetValue(CustomDatesProperty);
            set => SetValue(CustomDatesProperty, value);
        }

        private void OnUpdate()
        {
            if (CustomDates.IsFill)
            {
                SelectedDate = CustomDates.DateFrom.Value;
                SelectedDates.AddRange(CustomDates.DateFrom.Value, CustomDates.DateTo.Value);
            }
        }

        private static void OnChangeDates(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = (CalendarRangeExtension)d;
            o.CustomDates.OnSet = o.OnUpdate;
        }
    }
}