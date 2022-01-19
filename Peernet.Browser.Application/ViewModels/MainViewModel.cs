using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IModalNavigationService modalNavigationService;
        private readonly INotificationsManager notificationsManager;
        private readonly IApplicationManager applicationManager;
        private readonly FooterViewModel footerViewModel;
        private readonly NavigationBarViewModel navigationBarViewModel;

        public MainViewModel(
            FooterViewModel footerViewModel,
            NavigationBarViewModel navigationBarViewModel,
            INavigationService navigationService,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IApplicationManager applicationManager)
        {
            this.footerViewModel = footerViewModel;
            this.navigationBarViewModel = navigationBarViewModel;
            this.navigationService = navigationService;
            this.modalNavigationService = modalNavigationService;
            this.notificationsManager = notificationsManager;
            this.applicationManager = applicationManager;

            navigationService.StateChanged += Navigated;
            modalNavigationService.StateChanged += ModalNavigated;

            navigationService.Navigate<HomeViewModel>();
        }

        public ViewModelBase CurrentViewModel => navigationService.CurrentViewModel;

        public ViewModelBase CurrentModalViewModel => modalNavigationService.CurrentViewModel;

        public bool IsModalOpened => modalNavigationService.IsOpen;

        public NotificationCollection Notifications => notificationsManager.Notifications;

        public FooterViewModel Footer => footerViewModel;

        public NavigationBarViewModel NavBar => navigationBarViewModel;

        public override void Dispose()
        {
            navigationService.StateChanged -= Navigated;
            modalNavigationService.StateChanged -= ModalNavigated;

            base.Dispose();
        }

        private void Navigated()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void ModalNavigated()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpened));
        }

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
    }
}
