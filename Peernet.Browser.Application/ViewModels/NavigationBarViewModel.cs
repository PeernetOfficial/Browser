using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService navigationService;

        public NavigationBarViewModel(IMvxNavigationService navigationService, IUserContext userContext)
        {
            this.navigationService = navigationService;
            UserContext = userContext;

            NavigateExploreCommand = new MvxCommand(() => Navigate<ExploreViewModel>());
            NavigateHomeCommand = new MvxCommand(() => Navigate<HomeViewModel>(false));
            NavigateDirectoryCommand = new MvxCommand(() => Navigate<DirectoryViewModel>());

            EditProfileCommand = new MvxAsyncCommand(() =>
            {
                GlobalContext.IsMainWindowActive = false;
                GlobalContext.IsProfileMenuVisible = false;
                navigationService.Navigate<EditProfileViewModel>();

                return Task.CompletedTask;
            });

            NavigateAboutCommand = new MvxAsyncCommand(async () =>
            {
                GlobalContext.IsProfileMenuVisible = false;
                await navigationService.Navigate<AboutViewModel>();
            });
        }

        private Type actualActiveViewModel = typeof(HomeViewModel);

        private void Navigate<T>(bool showLogo = true) where T : IMvxViewModel
        {
            if (typeof(T) == actualActiveViewModel) return;
            actualActiveViewModel = typeof(T);
            navigationService.Navigate<T>();
            GlobalContext.IsLogoVisible = showLogo;
        }

        public IMvxCommand NavigateDirectoryCommand { get; }

        public IMvxCommand NavigateExploreCommand { get; }

        public IMvxCommand NavigateHomeCommand { get; }

        public IMvxAsyncCommand EditProfileCommand { get; }

        public IMvxAsyncCommand NavigateAboutCommand { get; }

        public IUserContext UserContext { get; set; }
    }
}