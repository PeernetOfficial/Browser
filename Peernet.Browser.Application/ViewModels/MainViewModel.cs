using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;

namespace Peernet.Browser.Application.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IModalNavigationService modalNavigationService;
        private readonly INotificationsManager notificationsManager;

        public MainViewModel(INavigationService navigationService, IModalNavigationService modalNavigationService, INotificationsManager notificationsManager)
        {
            this.navigationService = navigationService;
            this.modalNavigationService = modalNavigationService;
            this.notificationsManager = notificationsManager;
            navigationService.StateChanged += Navigated;
            modalNavigationService.StateChanged += ModalNavigated;

            navigationService.Navigate<HomeViewModel>();
        }

        public ViewModelBase CurrentViewModel => navigationService.CurrentViewModel;

        public ViewModelBase CurrentModalViewModel => modalNavigationService.CurrentViewModel;

        public NotificationCollection Notifications => notificationsManager.Notifications;

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
        }
    }
}
