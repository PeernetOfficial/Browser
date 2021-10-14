using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService navigationService;

        public NavigationBarViewModel(IMvxNavigationService navigationService, IUserContext userContext)
        {
            this.navigationService = navigationService;
            NavigateExploreCommand = new MvxCommand(() => Navigate<ExploreViewModel>());
            NavigateHomeCommand = new MvxCommand(() => Navigate<HomeViewModel>(false));
            NavigateDirectoryCommand = new MvxCommand(() => Navigate<DirectoryViewModel>());
            GoToYourFilesCommand = new MvxAsyncCommand(() => Task.CompletedTask);
            OpenCloseProfileMenuCommand = new MvxAsyncCommand(() =>
            {
                GlobalContext.IsProfileMenuVisible ^= true;
                return Task.CompletedTask;
            });
            UserContext = userContext;
        }

        private void Navigate<T>(bool showLogo = true) where T : IMvxViewModel
        {
            navigationService.Navigate<T>();
            GlobalContext.IsLogoVisible = showLogo;
        }

        public IMvxAsyncCommand GoToYourFilesCommand { get; }

        public IMvxCommand NavigateDirectoryCommand { get; }

        public IMvxCommand NavigateExploreCommand { get; }

        public IMvxCommand NavigateHomeCommand { get; }

        public IMvxAsyncCommand OpenCloseProfileMenuCommand { get; }

        public IUserContext UserContext { get; set; }
    }
}