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

        public event PropertyChangedEventHandler PropertyChanged;
        
        private UserContext()
        {
        }

        public UserContext(IProfileService profileService, IMvxNavigationService mvxNavigationService)
        {
            this.profileService = profileService;
            this.mvxNavigationService = mvxNavigationService;

            ReloadContext();
            User.PropertyChanged += SubscribeToUserModifications;
        }

        public List<MenuItemViewModel> Items { get; private set; }

        public User User { get; private set; }

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

        public UserContext GetSnapshot()
        {
            return new UserContext(profileService, mvxNavigationService)
            {
                HasUserChanged = HasUserChanged,
                Items = new UserContext { Items = Items }.Items,
                User = new UserContext { User = User.GetClone() }.User,
            };
        }
    }
}