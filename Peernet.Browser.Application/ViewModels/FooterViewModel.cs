using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Clients;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class FooterViewModel : MvxViewModel
    {
        private const int reconnectDelay = 2000;
        private readonly IApiService apiService;
        private readonly IApplicationManager applicationManager;
        private readonly IBlockchainService blockchainService;
        private readonly IMvxNavigationService navigationService;
        private readonly IWarehouseService warehouseService;
        private bool areDownloadsCollapsed;
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
            this.navigationService = navigationService;
            this.applicationManager = applicationManager;
            this.warehouseService = warehouseService;
            this.blockchainService = blockchainService;

            DownloadManager = downloadManager;
            DownloadManager.downloadsChanged += GetLastDownloadItem;
            UploadCommand = new MvxAsyncCommand(UploadFiles);
            DownloadManager.downloadsChanged += CollapseWhenSingleItem;

            Task.Run(UpdateStatuses);
        }

        public bool AreDownloadsCollapsed
        {
            get => areDownloadsCollapsed;
            set => SetProperty(ref areDownloadsCollapsed, value);
        }

        public IMvxAsyncCommand<string> CancelDownloadCommand => new MvxAsyncCommand<string>(
            async id =>
            {
                // Make API call and validate result
                await DownloadManager.CancelDownload(id);
            });

        public IMvxCommand CollapseExpandDownloadsCommand => new MvxCommand(() =>
            {
                AreDownloadsCollapsed ^= true;
            });

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

        public ObservableCollection<DownloadModel> ListedFileDownloads { get; set; } = new();

        public IMvxCommand OpenFileLocationCommand => new MvxCommand<string>(
            name =>
            {
                DownloadManager.OpenFileLocation(name);
            });

        public IMvxCommand PauseDownloadCommand => new MvxAsyncCommand<string>(
            async id =>
            {
                // Make API call and validate result
                await DownloadManager.PauseDownload(id);
            });

        public string Peers
        {
            get => peers;
            set => SetProperty(ref peers, value);
        }

        public IMvxCommand ResumeDownloadCommand => new MvxAsyncCommand<string>(
            async id =>
            {
                // Make API call and validate result
                await DownloadManager.ResumeDownload(id);
            });

        public IMvxAsyncCommand UploadCommand { get; }

        public override async Task Initialize()
        {
            await ConnectToPeernetAPI();
        }

        private void CollapseWhenSingleItem(object? sender, EventArgs e)
        {
            if (DownloadManager.ActiveFileDownloads.Count == 1)
            {
                AreDownloadsCollapsed = true;
            }
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

        private void GetLastDownloadItem(object sender, EventArgs e)
        {
            var copy = ListedFileDownloads.ToList();
            copy.ForEach(i => ListedFileDownloads.Remove(i));
            ListedFileDownloads.Add(DownloadManager.ActiveFileDownloads.LastOrDefault());
        }

        private async Task<bool> GetPeernetStatus()
        {
            var status = await apiService.GetStatus();
            ConnectionStatus = status.IsConnected ? ConnectionStatus.Online : ConnectionStatus.Offline;
            Peers = status.CountPeerList.ToString();
            return status.IsConnected;
        }

        private async Task UpdateStatuses()
        {
            while (true)
            {
                var status = await apiService.GetStatus();
                Peers = status?.CountPeerList.ToString();
                await Task.Delay(3000);
            }
        }

        private async Task UploadFiles()
        {
            var fileModels = applicationManager.OpenFileDialog().Select(f => new FileModel(f)).ToList();

            if (fileModels.Count != 0)
            {
                var parameter = new ShareFileViewModelParameter(warehouseService, blockchainService)
                {
                    FileModels = fileModels
                };

                GlobalContext.IsMainWindowActive = false;
                await navigationService.Navigate<GenericFileViewModel, ShareFileViewModelParameter>(parameter);
            }
        }
    }
}