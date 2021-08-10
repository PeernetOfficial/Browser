using MvvmCross.Plugin.FieldBinding;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using System.Threading.Tasks;


namespace Peernet.Browser.Application.ViewModels
{
    public class StatusViewModel : MvxViewModel
    {
        private readonly IApiClient apiClient;

        public StatusViewModel(IApiClient apiClient)
        {
            this.apiClient = apiClient;

        }

        public readonly INotifyChange<string> Connected = new NotifyChange<string>("Connecting...");
        public readonly INotifyChange<string> Peers = new NotifyChange<string>();

        public async override void Start()
        {
            base.Start();

            await GetConnectionStatus();
        }

        private async Task GetConnectionStatus()
        {
            var status = await this.apiClient.GetStatus();
            
            Connected.Value = status.IsConnected ? "OnLine" : "OffLine";
            Peers.Value = $"{status.CountPeerList} Peers";
        }
        
        
        
        
    }
}
