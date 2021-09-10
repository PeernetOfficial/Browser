using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class FooterViewModel : MvxViewModel
    {
        private const int reconnectDelay = 2000;
        private readonly IApiClient apiClient;
        private readonly IMvxNavigationService navigationService;
        private readonly ISocketClient socketClient;
        private string commandLineInput;
        private string commandLineOutput;
        private ConnectionStatus connectionStatus = ConnectionStatus.Offline;
        private string peers;

        public FooterViewModel(IApiClient apiClient, ISocketClient socketClient, IMvxNavigationService navigationService)
        {
            this.apiClient = apiClient;
            this.socketClient = socketClient;
            this.navigationService = navigationService;
        }

        public string CommandLineInput
        {
            get => commandLineInput;
            set
            {
                SetProperty(ref commandLineInput, value);
            }
        }

        public string CommandLineOutput
        {
            get => commandLineOutput;
            set
            {
                SetProperty(ref commandLineOutput, value);
            }
        }

        public ConnectionStatus ConnectionStatus
        {
            get => connectionStatus;
            set
            {
                SetProperty(ref connectionStatus, value);
            }
        }

        public string Peers
        {
            get => peers;
            set
            {
                SetProperty(ref peers, value);
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

        private async Task ConnectToPeernetConsole()
        {
            await this.socketClient.Connect();

            CommandLineOutput = await this.socketClient.Receive();
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

        public void UploadFiles(string[] files)
        {
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            navigationService.Navigate<ModalViewModel, string[]>(files);
        }
    }
}