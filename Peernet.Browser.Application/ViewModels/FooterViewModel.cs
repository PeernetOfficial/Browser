using MvvmCross.Commands;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class FooterViewModel : MvxViewModel
    {
        private const string connectingText = "Connecting...";
        private const string onlineText = "OnLine";
        private const string offlineText = "OffLine";
        private const int reconnectDelay = 2000;

        private readonly IApiClient apiClient;
        private readonly ISocketClient socketClient;

        public FooterViewModel(IApiClient apiClient, ISocketClient socketClient)
        {
            this.apiClient = apiClient;
            this.socketClient = socketClient;
        }

        public readonly INotifyChange<string> Connected = new NotifyChange<string>();
        public readonly INotifyChange<string> Peers = new NotifyChange<string>();
        public readonly INotifyChange<string> CommandLineOutput = new NotifyChange<string>();
        public readonly INotifyChange<string> CommandLineInput = new NotifyChange<string>();

        public IMvxAsyncCommand SendToPeernetConsole
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await this.socketClient.Send(CommandLineInput.Value);
                    CommandLineInput.Value = string.Empty;
                    CommandLineOutput.Value = await this.socketClient.Receive();
                });
            }
        }

        public async override Task Initialize()
        {
            await ConnectToPeernetAPI();
            await ConnectToPeernetConsole();
        }

        private async Task ConnectToPeernetConsole()
        {
            await this.socketClient.Connect();

            CommandLineOutput.Value = await this.socketClient.Receive();
        }

        private async Task ConnectToPeernetAPI()
        {
            var success = false;
            while (!success)
            {
                success = await GetPeernetStatus();

                if (!success)
                {
                    await Task.Delay(reconnectDelay);
                }
            }
        }

        private async Task<bool> GetPeernetStatus()
        {
            Connected.Value = connectingText;

            try
            {
                var status = await this.apiClient.GetStatus();

                Connected.Value = status.IsConnected ? onlineText : offlineText;
                Peers.Value = $"{status.CountPeerList} Peers";

                return status.IsConnected;
            }
            catch (System.Net.WebException)
            {
                Connected.Value = offlineText;
                Peers.Value = string.Empty;

                return false;
            }
        }

    }
}
