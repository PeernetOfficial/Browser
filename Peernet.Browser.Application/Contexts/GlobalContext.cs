using System;
using System.ComponentModel;

namespace Peernet.Browser.Application.Contexts
{
    public class GlobalContext : INotifyPropertyChanged
    {
        private static bool isMainWindowActive = true;

        private static bool isProfileMenuVisible;

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

        public static bool IsProfileMenuVisible
        {
            get => isProfileMenuVisible;
            set
            {
                isProfileMenuVisible = value;
                NotifyStaticPropertyChanged(nameof(IsProfileMenuVisible));
            }
        }

        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}