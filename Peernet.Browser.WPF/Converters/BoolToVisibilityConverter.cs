using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Peernet.Browser.WPF.Converters
{
    internal class BoolToVisibilityConverter : IValueConverter
    {
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

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = (bool)value;
            if (IsVisibilityInverted(parameter))
            {
                isVisible = !isVisible;
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Enum.Parse<Visibility>(value?.ToString());
            bool isVisible = visibility == Visibility.Visible;

            if (IsVisibilityInverted(parameter))
            {
                isVisible = !isVisible;
            }

            return isVisible;
        }
    }
}