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
            UserContext = userContext;

            NavigateExploreCommand = new MvxAsyncCommand(async () => await Navigate<ExploreViewModel>());
            NavigateHomeCommand = new MvxAsyncCommand(async () => await Navigate<HomeViewModel>(false));
            NavigateDirectoryCommand = new MvxAsyncCommand(async () => await Navigate<DirectoryViewModel>());

            EditProfileCommand = new MvxAsyncCommand(async () =>
            {
                GlobalContext.IsMainWindowActive = false;
                GlobalContext.IsProfileMenuVisible = false;
                await navigationService.Navigate<EditProfileViewModel>();
            });

            NavigateAboutCommand = new MvxAsyncCommand(async () =>
            {
                GlobalContext.IsProfileMenuVisible = false;
                await Navigate<AboutViewModel>();
            });

            OpenCloseProfileMenuCommand = new MvxAsyncCommand(() =>
            {
                GlobalContext.IsProfileMenuVisible ^= true;
                return Task.CompletedTask;
            });
        }

        public IMvxAsyncCommand EditProfileCommand { get; }

        public IMvxAsyncCommand NavigateAboutCommand { get; }

        public IMvxAsyncCommand NavigateDirectoryCommand { get; }

        public IMvxAsyncCommand NavigateExploreCommand { get; }

        public IMvxAsyncCommand NavigateHomeCommand { get; }

        public IMvxAsyncCommand OpenCloseProfileMenuCommand { get; }

        public IUserContext UserContext { get; set; }

        private async Task Navigate<T>(bool showLogo = true) where T : IMvxViewModel
        {
            if (typeof(T).Name == GlobalContext.CurrentViewModel)
            {
                return;
            }

            GlobalContext.CurrentViewModel = nameof(T);
            await navigationService.Navigate<T>();
            GlobalContext.IsLogoVisible = showLogo;
        }
    }
}