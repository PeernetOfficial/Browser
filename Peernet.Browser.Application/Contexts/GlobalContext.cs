using MvvmCross.Base;
using Peernet.Browser.Models.Presentation;
using System;
using System.ComponentModel;

namespace Peernet.Browser.Application.Contexts
{
    public class GlobalContext : INotifyPropertyChanged
    {
        private static bool isMainWindowActive = true;

        private static bool isLogoVisible;

        private static bool isProfileMenuVisible;

        private static VisualMode visualMode;

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
                NotifyStaticPropertyChanged(nameof(IsLogoVisible));
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