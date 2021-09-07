using MvvmCross.Navigation;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Peernet.Browser.Application.Contexts
{
    public class UserContext : IUserContext
    {
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IProfileService profileService;
        private Lazy<List<MenuItemViewModel>> menuItemViewModels;
        private Lazy<User> user;

        public UserContext(IProfileService profileService, IMvxNavigationService mvxNavigationService)
        {
            this.profileService = profileService;
            this.mvxNavigationService = mvxNavigationService;

            user = new Lazy<User>(() => InitializeUser());
            menuItemViewModels = new Lazy<List<MenuItemViewModel>>(() => InitializeMenuItems());
        }

        public List<MenuItemViewModel> Items => menuItemViewModels.Value;

        public User User => user.Value;

        private List<MenuItemViewModel> InitializeMenuItems()
        {
            return new List<MenuItemViewModel>
            {
                new MenuItemViewModel("About"),
                new MenuItemViewModel("FAQ (Help)"),
                new MenuItemViewModel("Backup to a file"),
                new MenuItemViewModel(
                    "Edit profile",
                    () =>
                    {
                        GlobalContext.IsMainWindowActive = false;
                        GlobalContext.IsProfileMenuVisible = false;
                        mvxNavigationService.Navigate<EditProfileViewModel>();
                    })
                };
        }

        private User InitializeUser()
        {
            return new User
            {
                Name = profileService.GetUserName(),
                Image = profileService.GetUserImage()
            };
        }
    }
}