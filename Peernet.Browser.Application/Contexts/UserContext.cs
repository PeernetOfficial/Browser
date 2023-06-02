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
        private readonly IAccountService accountService;
        private readonly IProfileService profileService;
        private User user;
        private string nodeId;
        private string peerId;

        public UserContext(IAccountService accountService, IProfileService profileService)
        {
            this.accountService = accountService;
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

        public string PeerId
        {
            get => peerId;
            set
            {
                peerId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PeerId)));
            }
        }

        public string NodeId
        {
            get => nodeId;
            set
            {
                nodeId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NodeId)));
            }
        }

        public void ReloadContext()
        {
            // Needs to be placed on the ThreadPool to avoid deadlock
            var selfPeer = Task.Run(async () => await accountService.Info()).GetResultBlockingWithoutContextSynchronization();
            PeerId = selfPeer.PeerId;
            NodeId = selfPeer.NodeId;
            User = Task.Run(async () => await profileService.GetUser()).GetResultBlockingWithoutContextSynchronization();
            HasUserChanged = false;
        }

        public void SubscribeToUserModifications(object sender, PropertyChangedEventArgs e)
        {
            HasUserChanged = true;
        }
    }
}