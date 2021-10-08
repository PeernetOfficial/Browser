using MvvmCross.Base;
using System;
using System.ComponentModel;

namespace Peernet.Browser.Application.Contexts
{
    public class GlobalContext : INotifyPropertyChanged
    {
        private static bool isMainWindowActive = true;

        private static bool isLogoVisible;

        private static bool isProfileMenuVisible;

        private static NotificationCollection notifications;

        private static string currentViewModel;

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged;

        public static IMvxMainThreadAsyncDispatcher UiThreadDispatcher { get; set; }

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
                NotifyStaticPropertyChanged(nameof(isLogoVisible));
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

        public static string CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                NotifyStaticPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public static NotificationCollection Notifications
        {
            get => notifications ??= new NotificationCollection(11000);
            set
            {
                notifications = value;
                NotifyStaticPropertyChanged(nameof(Notifications));
            }
        }

        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}