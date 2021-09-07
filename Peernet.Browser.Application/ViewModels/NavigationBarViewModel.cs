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

        private bool isProfileMenuVisible;

        public NavigationBarViewModel(IMvxNavigationService navigationService, IUserContext userContext)
        {
            this.navigationService = navigationService;

            UserContext = userContext;
        }

        public IMvxAsyncCommand GoToYourFilesCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    return Task.CompletedTask;
                });
            }
        }

        public bool IsProfileMenuVisible
        {
            get => isProfileMenuVisible;
            set
            {
                isProfileMenuVisible = value;
                RaisePropertyChanged(nameof(IsProfileMenuVisible));
            }
        }

        public IMvxAsyncCommand NavigateDirectoryCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await navigationService.Navigate<DirectoryViewModel>();
                });
            }
        }

        public IMvxAsyncCommand NavigateExploreCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await navigationService.Navigate<ExploreViewModel>();
                });
            }
        }

        public IMvxAsyncCommand NavigateHomeCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await navigationService.Navigate<HomeViewModel>();
                });
            }
        }

        public IMvxAsyncCommand OpenCloseProfileMenuCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    IsProfileMenuVisible ^= true;

                    return Task.CompletedTask;
                });
            }
        }

        public IUserContext UserContext { get; set; }
    }
}