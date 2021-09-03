using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace Peernet.Browser.WPF.Converters
{
    internal class BoolToVisibilityConverter : MvxNativeValueConverter<BoolToVisibilityValueConverter>
    {
    }

    internal class BoolToVisibilityValueConverter : MvxValueConverter<bool, Visibility>
    {
        protected override Visibility Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}