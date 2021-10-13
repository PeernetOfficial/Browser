using MvvmCross.Navigation;
using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Models.Presentation.Profile;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Peernet.Browser.Application.Services;

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

        // todo: it should be asynchronous method
        public void ReloadContext()
        {
            // Needs to be placed on the ThreadPool to avoid deadlock
            User = Task.Run(async () => await profileService.GetUser()).GetResultBlockingWithoutContextSynchronization();
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
                new("About", () => mvxNavigationService.Navigate<AboutViewModel>()),
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
    }
}