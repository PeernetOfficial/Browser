using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Navigation;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public NavigationBarViewModel(INavigationService navigationService, IUserContext userContext)
        {
            this.navigationService = navigationService;
            UserContext = userContext;

            NavigateExploreCommand = new AsyncCommand(() =>
            {
                Navigate<ExploreViewModel>();

                return Task.CompletedTask;
            });

            NavigateHomeCommand = new AsyncCommand(() =>
            {
                if (GlobalContext.CurrentViewModel != nameof(HomeViewModel))
                {
                    Navigate<HomeViewModel>(false);
                }

                return Task.CompletedTask;
            });

            NavigateDirectoryCommand = new AsyncCommand(() =>
            {
                Navigate<DirectoryViewModel>();
                return Task.CompletedTask;
            });

            EditProfileCommand = new AsyncCommand(() =>
            {
                GlobalContext.IsMainWindowActive = false;
                GlobalContext.IsProfileMenuVisible = false;
                navigationService.Navigate<EditProfileViewModel>();

                return Task.CompletedTask;
            });

            NavigateAboutCommand = new AsyncCommand(() =>
            {
                GlobalContext.IsProfileMenuVisible = false;
                Navigate<AboutViewModel>();
                return Task.CompletedTask;
            });

            OpenCloseProfileMenuCommand = new AsyncCommand(() =>
            {
                GlobalContext.IsProfileMenuVisible ^= true;
                return Task.CompletedTask;
            });
        }

        public IAsyncCommand EditProfileCommand { get; }

        public IAsyncCommand NavigateAboutCommand { get; }

        public IAsyncCommand NavigateDirectoryCommand { get; }

        public IAsyncCommand NavigateExploreCommand { get; }

        public IAsyncCommand NavigateHomeCommand { get; }

        public IAsyncCommand OpenCloseProfileMenuCommand { get; }

        public IUserContext UserContext { get; set; }

        private void Navigate<T>(bool showLogo = true) where T : ViewModelBase
        {
            if (typeof(T).Name == GlobalContext.CurrentViewModel)
            {
                return;
            }

            GlobalContext.CurrentViewModel = nameof(T);
            navigationService.Navigate<T>();
            GlobalContext.IsLogoVisible = showLogo;
        }
    }
}