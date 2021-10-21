using System;
using System.Globalization;
using System.Windows.Data;

namespace Peernet.Browser.WPF.Converters
{
    public class LookupConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var date = (DateTime)values[0];
                var from = (DateTime?)values[1];
                var to = (DateTime?)values[2];
                return from == date || to == date;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}