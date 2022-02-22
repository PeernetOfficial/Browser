using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Application.Services;
using Peernet.SDK.Models.Presentation.Profile;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Contexts
{
    public class UserContext : INotifyPropertyChanged, IUserContext
    {
        private readonly IProfileService profileService;
        private User user;

        public UserContext(IProfileService profileService)
        {
            this.profileService = profileService;

            try
            {
                ReloadContext();
                GlobalContext.IsConnected = true;
            }
            catch (HttpRequestException e)
            {
                GlobalContext.ErrorMessage = $"{e.Message}. \nReview the configuration and restart the App.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasUserChanged { get; set; }

        public User User
        {
            get => user;
            set
            {
                user = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
                User.PropertyChanged += SubscribeToUserModifications;
            }
        }

        public void ReloadContext()
        {
            // Needs to be placed on the ThreadPool to avoid deadlock
            User = Task.Run(async () => await profileService.GetUser()).GetResultBlockingWithoutContextSynchronization();
            HasUserChanged = false;
        }

        public void SubscribeToUserModifications(object sender, PropertyChangedEventArgs e)
        {
            HasUserChanged = true;
        }
    }
}