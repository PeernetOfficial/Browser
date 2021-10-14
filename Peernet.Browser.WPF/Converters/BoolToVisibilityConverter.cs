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
            if (IsVisibilityInverted(parameter))
                value = !value;

            return value ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override bool ConvertBack(Visibility value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = value == Visibility.Visible;

            if (IsVisibilityInverted(parameter))
            {
                isVisible = !isVisible;
            }

            return isVisible;
        }

        private static Visibility GetVisibilityMode(object parameter)
        {
            Visibility mode = Visibility.Visible;

            if (parameter != null)
            {
                if (parameter is Visibility)
                {
                    mode = (Visibility)parameter;
                }
                else
                {
                    try
                    {
                        mode = (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString(), true);
                    }
                    catch (FormatException e)
                    {
                        throw new FormatException("Invalid Visibility specified as the ConverterParameter. Use Visible or Collapsed.", e);
                    }
                }
            }

            return mode;
        }

        private static bool IsVisibilityInverted(object parameter)
        {
            return GetVisibilityMode(parameter) == Visibility.Collapsed;
        }
    }
}