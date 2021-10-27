using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Models.Presentation.Footer;
using System.Linq;
using System.Threading.Tasks;
using Peernet.Browser.Application.Clients;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Application.ViewModels
{
    public class FooterViewModel : MvxViewModel
    {
        private const int reconnectDelay = 2000;
        private readonly IApiService apiService;
        private readonly IApplicationManager applicationManager;
        private readonly IMvxNavigationService navigationService;
        private readonly ISocketClient socketClient;
        private readonly IWarehouseService warehouseService;
        private readonly IBlockchainService blockchainService;
        private string commandLineInput;
        private string commandLineOutput;
        private ConnectionStatus connectionStatus = ConnectionStatus.Offline;
        private string peers;

        public FooterViewModel(
            IApiService apiService,
            ISocketClient socketClient,
            IMvxNavigationService navigationService,
            IApplicationManager applicationManager,
            IDownloadManager downloadManager,
            IWarehouseService warehouseService,
            IBlockchainService blockchainService)
        {
            this.apiService = apiService;
            this.socketClient = socketClient;
            this.navigationService = navigationService;
            this.applicationManager = applicationManager;
            this.warehouseService = warehouseService;
            this.blockchainService = blockchainService;
            DownloadManager = downloadManager;

            UploadCommand = new MvxCommand(UploadFiles);
            SendToPeernetConsole = new MvxAsyncCommand(SendToPeernetMethod);
        }

        public string CommandLineInput
        {
            get => commandLineInput;
            set => SetProperty(ref commandLineInput, value);
        }

        public string CommandLineOutput
        {
            get => commandLineOutput;
            set => SetProperty(ref commandLineOutput, value);
        }

        public ConnectionStatus ConnectionStatus
        {
            get => connectionStatus;
            set => SetProperty(ref connectionStatus, value);
        }

        public IDownloadManager DownloadManager { get; }

        public string Peers
        {
            get => peers;
            set => SetProperty(ref peers, value);
        }

        public IMvxAsyncCommand SendToPeernetConsole { get; }

        public IMvxCommand UploadCommand { get; }

        public IMvxCommand PauseDownloadCommand => new MvxAsyncCommand<string>(
            async id =>
            {
                // Make API call and validate result
                await DownloadManager.PauseDownload(id);
            });

        public IMvxCommand ResumeDownloadCommand => new MvxAsyncCommand<string>(
            async id =>
            {
                // Make API call and validate result
                await DownloadManager.ResumeDownload(id);
            });

        public IMvxAsyncCommand<string> CancelDownloadCommand => new MvxAsyncCommand<string>(
            async id =>
            {
                // Make API call and validate result
                await DownloadManager.CancelDownload(id);
            });

        public override async Task Initialize()
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
            await socketClient.Connect();
            CommandLineOutput = await socketClient.Receive();
        }

        private async Task<bool> GetPeernetStatus()
        {
            var status = await apiService.GetStatus();
            ConnectionStatus = status.IsConnected ? ConnectionStatus.Online : ConnectionStatus.Offline;
            Peers = status.CountPeerList.ToString();
            return status.IsConnected;
        }

        private async Task SendToPeernetMethod()
        {
            await socketClient.Send(CommandLineInput);
            CommandLineInput = string.Empty;
            CommandLineOutput = await socketClient.Receive();
        }

        private void UploadFiles()
        {
            var f = applicationManager.OpenFileDialog().Select(f => new FileModel(f)).ToArray();
            if (f.Length != 0)
            {
                var parameter = new ShareFileViewModelParameter(warehouseService, blockchainService)
                {
                    FileModels = f
                };

                GlobalContext.IsMainWindowActive = false;
                navigationService.Navigate<GenericFileViewModel, ShareFileViewModelParameter>(parameter);
            }
        }
    }
}