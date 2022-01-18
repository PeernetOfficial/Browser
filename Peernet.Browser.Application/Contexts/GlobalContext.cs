using Peernet.Browser.Models.Presentation;
using System;
using System.ComponentModel;
using System.Threading;

namespace Peernet.Browser.Application.Contexts
{
    public class GlobalContext : INotifyPropertyChanged
    {
        private static bool isMainWindowActive = true;

        private static bool isLogoVisible;

        private static bool isConnected;

        private static bool isProfileMenuVisible;

        private static VisualMode visualMode;

        private static string currentViewModel;

        private static string errorMessage;

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged;

        public static bool IsMainWindowActive
        {
            get => isMainWindowActive;
            set
            {
                isMainWindowActive = value;
                NotifyStaticPropertyChanged(nameof(IsMainWindowActive));
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
        
        public static bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                NotifyStaticPropertyChanged(nameof(isConnected));
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

        public static bool IsProfileMenuVisible
        {
            get => isProfileMenuVisible;
            set
            {
                isProfileMenuVisible = value;
                NotifyStaticPropertyChanged(nameof(IsProfileMenuVisible));
            }
        }

        private static object modal;

        public static object Modal
        {
            get => modal;
            set
            {
                modal = value;
                NotifyStaticPropertyChanged(nameof(modal));
            }
        }

        public static string CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                NotifyStaticPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public static string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyStaticPropertyChanged(nameof(errorMessage));
            }
        }

        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}