using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : MvxViewModel
    {
        public NavigationBarViewModel(IMvxNavigationService navigationService, IUserContext userContext)
        {
            NavigateExploreCommand = new MvxCommand(() => navigationService.Navigate<ExploreViewModel>());
            NavigateHomeCommand = new MvxCommand(() => navigationService.Navigate<HomeViewModel>());
            NavigateDirectoryCommand = new MvxCommand(() => navigationService.Navigate<DirectoryViewModel>());
            GoToYourFilesCommand = new MvxAsyncCommand(() => Task.CompletedTask);
            OpenCloseProfileMenuCommand = new MvxAsyncCommand(() =>
            {
                GlobalContext.IsProfileMenuVisible ^= true;
                return Task.CompletedTask;
            });
            UserContext = userContext;
        }

        public IMvxAsyncCommand GoToYourFilesCommand { get; }

        public IMvxCommand NavigateDirectoryCommand { get; }

        public IMvxCommand NavigateExploreCommand { get; }

        public IMvxCommand NavigateHomeCommand { get; }

        public IMvxAsyncCommand OpenCloseProfileMenuCommand { get; }

        public IUserContext UserContext { get; set; }
    }
}