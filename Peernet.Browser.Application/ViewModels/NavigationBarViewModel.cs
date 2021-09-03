using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService navigationService;
        private bool isProfileMenuVisible;

        public bool IsProfileMenuVisible
        {
            get => isProfileMenuVisible;
            set
            {
                isProfileMenuVisible = value;
                RaisePropertyChanged(nameof(IsProfileMenuVisible));
            }
        }

        public List<MenuItemViewModel> Items { get; set; }


        public NavigationBarViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
            Items = new List<MenuItemViewModel>
            {
                new MenuItemViewModel("About"),
                new MenuItemViewModel("FAQ (Help)"),
                new MenuItemViewModel("Backup to a file")
            };
        }

        public IMvxAsyncCommand NavigateHomeCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    await navigationService.Navigate<HomeViewModel>();
                });
            }
        }

        public IMvxAsyncCommand NavigateUserCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    return Task.CompletedTask;
                });
            }
        }

        public IMvxAsyncCommand NavigateDictionaryCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    return Task.CompletedTask;
                });
            }
        }

        public IMvxAsyncCommand OpenProfileMenuCommand
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
    }
}
