using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Peernet.Browser.WPF.Converters
{
    public class TextInputToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is bool hasNoText && values[1] is bool hasFocus)
            {
                if (hasFocus || !hasNoText) return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}