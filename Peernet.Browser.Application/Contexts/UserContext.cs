using MvvmCross.Navigation;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace Peernet.Browser.Application.Contexts
{
    public class UserContext : INotifyPropertyChanged, IUserContext
    {
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IProfileService profileService;
        private User user;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserContext(IProfileService profileService, IMvxNavigationService mvxNavigationService)
        {
            this.profileService = profileService;
            this.mvxNavigationService = mvxNavigationService;

            ReloadContext();
            User.PropertyChanged += SubscribeToUserModifications;
        }

        public List<MenuItemViewModel> Items { get; private set; }

        public User User
        {
            get => user; set
            {
                user = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
            }
        }

        public bool HasUserChanged { get; private set; }

        public void ReloadContext()
        {
            User = InitializeUser();
            Items = InitializeMenuItems();
        }

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

        public void SubscribeToUserModifications(object sender, PropertyChangedEventArgs e)
        {
            HasUserChanged = true;
        }
    }
}