using Peernet.Browser.Application.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

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