using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Profile;
using System.ComponentModel;
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

            ReloadContext();
            User.PropertyChanged += SubscribeToUserModifications;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasUserChanged { get; private set; }

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
        }

        public void SubscribeToUserModifications(object sender, PropertyChangedEventArgs e)
        {
            HasUserChanged = true;
        }
    }
}