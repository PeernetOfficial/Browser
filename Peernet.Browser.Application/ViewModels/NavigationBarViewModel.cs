using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService navigationService;
        private readonly IProfileService profileService;
        private bool isProfileMenuVisible;

        // To be replaced with some Service fed data
        private User user;

        public User User
        {
            get => user;
            set
            {
                user = value;
                RaisePropertyChanged(nameof(User));
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

        public List<MenuItemViewModel> Items { get; set; }

        public NavigationBarViewModel(IMvxNavigationService navigationService, IProfileService profileService)
        {
            this.navigationService = navigationService;
            this.profileService = profileService;

            InitializeContext();
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

        private void InitializeContext()
        {
            User = new User
            {
                Name = profileService.GetUserName(),
                Image = profileService.GetUserImage()
            };

            Items = new List<MenuItemViewModel>
            {
                new MenuItemViewModel("About"),
                new MenuItemViewModel("FAQ (Help)"),
                new MenuItemViewModel("Backup to a file")
            };
        }
    }
}