using Peernet.SDK.Models.Presentation;
using System;
using System.ComponentModel;

namespace Peernet.Browser.Application.Contexts
{
    public class GlobalContext : INotifyPropertyChanged
    {
        private static string errorMessage;
        private static bool isConnected;
        private static bool isLogoVisible;
        private static bool isProfileMenuVisible;

        private static VisualMode visualMode;

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged;

        public static string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyStaticPropertyChanged(nameof(errorMessage));
            }
        }

        public static bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                NotifyStaticPropertyChanged(nameof(isConnected));
            }
        }

        public static bool IsLogoVisible
        {
            get => isLogoVisible;
            set
            {
                isLogoVisible = value;
                NotifyStaticPropertyChanged(nameof(IsLogoVisible));
            }
        }

        public static bool IsProfileMenuVisible
        {
            get => isProfileMenuVisible;
            set
            {
                isProfileMenuVisible = value;
                NotifyStaticPropertyChanged(nameof(IsProfileMenuVisible));
            }
        }

        public static VisualMode VisualMode
        {
            get => visualMode;
            set
            {
                visualMode = value;
                NotifyStaticPropertyChanged(nameof(VisualMode));
            }
        }

        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}