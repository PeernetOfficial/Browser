using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Widgets;
using Peernet.SDK.Models.Presentation.Widgets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IApplicationManager applicationManager;
        private readonly IWidgetsService widgetsService;
        private readonly FooterViewModel footerViewModel;
        private readonly IModalNavigationService modalNavigationService;
        private readonly NavigationBarViewModel navigationBarViewModel;
        private readonly INavigationService navigationService;
        private readonly INotificationsManager notificationsManager;
        private int selectedIndex = 0;

        public MainViewModel(
            FooterViewModel footerViewModel,
            HomeViewModel homeViewModel,
            AboutViewModel aboutViewModel,
            DirectoryViewModel directoryViewModel,
            ExploreViewModel exploreViewModel,
            NavigationBarViewModel navigationBarViewModel,
            INavigationService navigationService,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IApplicationManager applicationManager,
            IWidgetsService widgetsService)
        {
            this.footerViewModel = footerViewModel;
            this.navigationBarViewModel = navigationBarViewModel;
            this.navigationService = navigationService;
            this.modalNavigationService = modalNavigationService;
            this.notificationsManager = notificationsManager;
            this.applicationManager = applicationManager;
            this.widgetsService = widgetsService;

            AboutViewModel = aboutViewModel;
            DirectoryViewModel = directoryViewModel;
            HomeViewModel = homeViewModel;
            ExploreViewModel = exploreViewModel;

            navigationService.StateChanged += Navigated;
            modalNavigationService.StateChanged += ModalNavigated;

            navigationService.Navigate<HomeViewModel>();
        }
        
        public AboutViewModel AboutViewModel { get; private set; }

        public IAsyncCommand CloseAppCommand
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    CloseApp();

                    return Task.CompletedTask;
                });
            }
        }
        
        public ViewModelBase CurrentModalViewModel => modalNavigationService.CurrentViewModel;

        public ViewModelBase CurrentViewModel => navigationService.CurrentViewModel;

        public DirectoryViewModel DirectoryViewModel { get; private set; }

        public ExploreViewModel ExploreViewModel { get; private set; }

        public FooterViewModel Footer => footerViewModel;

        public HomeViewModel HomeViewModel { get; private set; }

        public bool IsModalOpened => modalNavigationService.IsOpen;

        public IAsyncCommand MaximizeCommand
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    Maximize();

                    return Task.CompletedTask;
                });
            }
        }

        public IAsyncCommand MinimizeCommand
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    Minimize();

                    return Task.CompletedTask;
                });
            }
        }

        public IWidgetsService WidgetsService => widgetsService;

        public NavigationBarViewModel NavBar => navigationBarViewModel;

        public NotificationCollection Notifications => notificationsManager.Notifications;

        public Action OpenAboutTab { get; set; }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        public override void Dispose()
        {
            navigationService.StateChanged -= Navigated;
            modalNavigationService.StateChanged -= ModalNavigated;

            base.Dispose();
        }

        private void CloseApp()
        {
            applicationManager.Shutdown();
        }

        private void Maximize()
        {
            if (applicationManager.IsMaximized)
            {
                applicationManager.Restore();
            }
            else
            {
                applicationManager.Maximize();
            }
        }

        private void Minimize()
        {
            applicationManager.Minimize();
        }

        private void ModalNavigated()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpened));
        }

        private void Navigated()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}