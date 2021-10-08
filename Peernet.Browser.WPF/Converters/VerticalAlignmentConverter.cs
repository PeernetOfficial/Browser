using Peernet.Browser.Models.Presentation;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Peernet.Browser.Models.Presentation.Home;

namespace Peernet.Browser.WPF.Converters
{
    public class VerticalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Alignments v && v == Alignments.Center) return VerticalAlignment.Center;
            return VerticalAlignment.Stretch;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}