using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models.Presentation;
using System.Linq;
using System.Threading.Tasks;
using Peernet.Browser.Application.Facades;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.ViewModels
{
    public class FooterViewModel : MvxViewModel
    {
        private const int reconnectDelay = 2000;
        private readonly IApiFacade apiFacade;
        private readonly IApplicationManager applicationManager;
        private readonly IMvxNavigationService navigationService;
        private readonly ISocketClient socketClient;
        private string commandLineInput;
        private string commandLineOutput;
        private ConnectionStatus connectionStatus = ConnectionStatus.Offline;
        private string peers;

        public FooterViewModel(IApiFacade apiFacade, ISocketClient socketClient, IMvxNavigationService navigationService, IApplicationManager applicationManager, IDownloadManager downloadManager)
        {
            this.apiFacade = apiFacade;
            this.socketClient = socketClient;
            this.navigationService = navigationService;
            this.applicationManager = applicationManager;
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
            id =>
            {
                // Make API call and validate result
                DownloadManager.PauseDownload(id);
                // This call shouldn't be made(either pause or dequeue should do all the job) - wait for the specification
                DownloadManager.DequeueDownload(id);

                return Task.CompletedTask;
            });

        public IMvxAsyncCommand<string> CancelDownloadCommand => new MvxAsyncCommand<string>(
            id =>
            {
                // Make API call and validate result
                DownloadManager.CancelDownload(id);
                // This call shouldn't be made(either cancel or dequeue should do all the job) - wait for the specification
                DownloadManager.DequeueDownload(id);

                return Task.CompletedTask;
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
            var status = await apiFacade.GetStatus();
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
            var f = applicationManager.OpenFileDialog();
            if (!f.Any()) return;
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            navigationService.Navigate<ShareNewFileViewModel, string[]>(f);
        }
    }
}