using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Peernet.Browser.WPF.Converters
{
    class BoolToVisibilityConverter : MvxNativeValueConverter<BoolToVisibilityValueConverter>
    {   
    }

    class BoolToVisibilityValueConverter : MvxValueConverter<bool, Visibility>
    {
        protected override Visibility Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
