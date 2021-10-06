using MvvmCross.Navigation;
using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Models.Extensions;
using Peernet.Browser.Models.Presentation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Peernet.Browser.Application.Extensions;

namespace Peernet.Browser.Application.Contexts
{
    public class UserContext : INotifyPropertyChanged, IUserContext
    {
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IProfileFacade profileFacade;
        private User user;

        public UserContext(IProfileFacade profileFacade, IMvxNavigationService mvxNavigationService)
        {
            this.profileFacade = profileFacade;
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
            // Needs to be placed on the ThreadPool to avoid deadlock
            User = Task.Run(async () => await profileFacade.GetUser()).GetResultBlockingWithoutContextSynchronization();
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
    }
}