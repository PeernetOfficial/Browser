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

        public UserContext(IProfileService profileService, IMvxNavigationService mvxNavigationService)
        {
            this.profileService = profileService;
            this.mvxNavigationService = mvxNavigationService;

            ReloadContext();
            User.PropertyChanged += SubscribeToUserModifications;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public bool HasUserChanged { get; private set; }
        
        public List<MenuItemViewModel> Items { get; private set; }

        public User User
        {
            get => user;
            set
            {
                user = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
            }
        }

        public void ReloadContext()
        {
            User = InitializeUser();
            Items = InitializeMenuItems();
        }

        public void SubscribeToUserModifications(object sender, PropertyChangedEventArgs e)
        {
            HasUserChanged = true;
        }

        private List<MenuItemViewModel> InitializeMenuItems()
        {
            return new List<MenuItemViewModel>
            {
                new("About"),
                new("FAQ (Help)"),
                new("Backup to a file"),
                new(
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
            var image = profileService.GetUserImage();
            var name = profileService.GetUserName();

            return new User
            {
                Name = string.IsNullOrEmpty(name) ? null : name,
                Image = image?.Length == 0 ? null : image
            };
        }
    }
}