using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class FooterViewModel : MvxViewModel
    {
        private const int reconnectDelay = 2000;

        private readonly IApiClient apiClient;
        private readonly ISocketClient socketClient;
        private ConnectionStatus connectionStatus = ConnectionStatus.Offline;
        private string peers;
        private string commandLineOutput;
        private string commandLineInput;

        public FooterViewModel(IApiClient apiClient, ISocketClient socketClient)
        {
            this.apiClient = apiClient;
            this.socketClient = socketClient;
        }

        public ConnectionStatus ConnectionStatus
        {
            get => connectionStatus;
            set
            {
                connectionStatus = value;
                RaisePropertyChanged(nameof(ConnectionStatus));
            }
        }

        public string Peers
        {
            get => peers;
            set
            {
                peers = value;
                RaisePropertyChanged(nameof(Peers));
            }
        }

        public string CommandLineOutput
        {
            get => commandLineOutput;
            set
            {
                commandLineOutput = value;
                RaisePropertyChanged(nameof(CommandLineOutput));
            }
        }

        public string CommandLineInput
        {
            get => commandLineInput;
            set
            {
                commandLineInput = value;
                RaisePropertyChanged(nameof(CommandLineInput));
            }
        }
        public IMvxAsyncCommand SendToPeernetConsole
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await this.socketClient.Send(CommandLineInput);
                    CommandLineInput = string.Empty;
                    CommandLineOutput = await this.socketClient.Receive();
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

            CommandLineOutput = await this.socketClient.Receive();
        }

        private async Task ConnectToPeernetAPI()
        {
            var success = false;
            while (!success)
            {
                ConnectionStatus = ConnectionStatus.Connecting;
                success = await GetPeernetStatus();

                if (!success)
                {
                    await Task.Delay(reconnectDelay);
                }
            }
        }

        private async Task<bool> GetPeernetStatus()
        {
            try
            {
                var status = await this.apiClient.GetStatus();
                ConnectionStatus = status.IsConnected ? ConnectionStatus.Online : ConnectionStatus.Offline;
                Peers = $"{status.CountPeerList} Peers";

                return status.IsConnected;
            }
            catch (System.Net.WebException)
            {
                Peers = string.Empty;
                ConnectionStatus = ConnectionStatus.Offline;

                return false;
            }
        }
    }
}