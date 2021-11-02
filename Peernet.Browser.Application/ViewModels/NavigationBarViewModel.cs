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

            NavigateExploreCommand = new MvxAsyncCommand(async () => await Navigate<ExploreViewModel>());
            NavigateHomeCommand = new MvxAsyncCommand(async () => await Navigate<HomeViewModel>(false));
            NavigateDirectoryCommand = new MvxAsyncCommand(async () => await Navigate<DirectoryViewModel>());

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
                await Navigate<AboutViewModel>();
            });
        }

        private Type actualActiveViewModel = typeof(HomeViewModel);

        private async Task Navigate<T>(bool showLogo = true) where T : IMvxViewModel
        {
            if (typeof(T) == actualActiveViewModel)
            {
                return;
            }

            actualActiveViewModel = typeof(T);
            await navigationService.Navigate<T>();
            GlobalContext.IsLogoVisible = showLogo;
        }

        public IMvxAsyncCommand NavigateDirectoryCommand { get; }

        public IMvxAsyncCommand NavigateExploreCommand { get; }

        public IMvxAsyncCommand NavigateHomeCommand { get; }

        public IMvxAsyncCommand EditProfileCommand { get; }

        public IMvxAsyncCommand NavigateAboutCommand { get; }

        public IUserContext UserContext { get; set; }
    }
}