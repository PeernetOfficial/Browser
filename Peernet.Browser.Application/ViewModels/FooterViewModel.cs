using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Domain.Status;
using Peernet.SDK.Models.Presentation;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class FooterViewModel : ViewModelBase
    {
        private const int reconnectDelay = 2000;
        private readonly IStatusService statusService;
        private readonly IApplicationManager applicationManager;
        private readonly IBlockchainService blockchainService;
        private readonly CurrentUserDirectoryViewModel currentUserDirectoryViewModel;
        private readonly IModalNavigationService modalNavigationService;
        private readonly INavigationService navigationService;
        private readonly INotificationsManager notificationsManager;
        private readonly ISettingsManager settingsManager;
        private readonly IWarehouseClient warehouseClient;
        private bool areDownloadsCollapsed;
        private string commandLineInput;
        private string commandLineOutput;
        private ConnectionStatus connectionStatus = ConnectionStatus.Offline;
        private int peers;
        private ObservableCollection<PeerStatus> peerStatuses;

        public FooterViewModel(
            IStatusService apiService,
            INavigationService navigationService,
            IModalNavigationService modalNavigationService,
            IApplicationManager applicationManager,
            IDataTransferManager dataTransferManager,
            IWarehouseClient warehouseClient,
            IBlockchainService blockchainService,
            ISettingsManager settingsManager,
            INotificationsManager notificationsManager,
            DirectoryViewModel directoryViewModel)
        {
            this.statusService = apiService;
            this.navigationService = navigationService;
            this.modalNavigationService = modalNavigationService;
            this.applicationManager = applicationManager;
            this.warehouseClient = warehouseClient;
            this.blockchainService = blockchainService;
            this.settingsManager = settingsManager;
            this.notificationsManager = notificationsManager;
            this.currentUserDirectoryViewModel = directoryViewModel.CurrentUserDirectoryViewModel;

            DataTransferManager = dataTransferManager;
            DataTransferManager.downloadsChanged += GetLastDownloadItem;
            DataTransferManager.downloadsChanged += CollapseWhenSingleItem;
            UploadCommand = new AsyncCommand(UploadFiles);

            Task.Run(ConnectToPeernetAPI).ConfigureAwait(false).GetAwaiter().GetResult();

            Task.Run(UpdateStatuses);
        }

        public bool AreDownloadsCollapsed
        {
            get => areDownloadsCollapsed;
            set
            {
                areDownloadsCollapsed = value;
                OnPropertyChanged(nameof(AreDownloadsCollapsed));
            }
        }

        public IAsyncCommand<Guid> CancelDownloadCommand => new AsyncCommand<Guid>(DataTransferManager.CancelTransfer);

        public IAsyncCommand CollapseExpandDownloadsCommand => new AsyncCommand(
            () =>
            {
                AreDownloadsCollapsed ^= true;
                return Task.CompletedTask;
            });

        public string CommandLineInput
        {
            get => commandLineInput;
            set
            {
                commandLineInput = value;
                OnPropertyChanged(nameof(CommandLineInput));
            }
        }

        public string CommandLineOutput
        {
            get => commandLineOutput;
            set
            {
                commandLineOutput = value;
                OnPropertyChanged(nameof(CommandLineOutput));
            }
        }

        public ConnectionStatus ConnectionStatus
        {
            get => connectionStatus;
            set
            {
                connectionStatus = value;
                OnPropertyChanged(nameof(ConnectionStatus));
            }
        }

        public IDataTransferManager DataTransferManager { get; }

        public ObservableCollection<DataTransfer> ListedFileDownloads { get; set; } = new();

        public IAsyncCommand<SDK.Models.Presentation.Download> OpenFileCommand => new AsyncCommand<SDK.Models.Presentation.Download>(
            download =>
            {
                if (download.Status == DataTransferStatus.Finished)
                {
                    var path = Path.Combine(settingsManager.DownloadPath, download.File.Name);
                    OpenWithDefaultProgram(path);
                }

                return Task.CompletedTask;
            });

        public IAsyncCommand<string> OpenFileLocationCommand => new AsyncCommand<string>(
            name =>
            {
                DataTransferManager.OpenFileLocation(name);

                return Task.CompletedTask;
            });

        public IAsyncCommand<Guid> PauseDownloadCommand => new AsyncCommand<Guid>(DataTransferManager.PauseTransfer);

        public int Peers
        {
            get => peers;
            set
            {
                peers = value;
                OnPropertyChanged(nameof(Peers));
            }
        }

        public ObservableCollection<PeerStatus> PeerStatuses
        {
            get => peerStatuses;
            set
            {
                peerStatuses = value;
                OnPropertyChanged(nameof(PeerStatuses));
            }
        }

        public IAsyncCommand<Guid> ResumeDownloadCommand => new AsyncCommand<Guid>(DataTransferManager.ResumeTransfer);

        public IAsyncCommand UploadCommand { get; }

        private static void OpenWithDefaultProgram(string path)
        {
            using var process = new Process();

            process.StartInfo.FileName = "explorer";
            process.StartInfo.Arguments = "\"" + path + "\"";
            process.Start();
        }

        private void CollapseWhenSingleItem(object sender, EventArgs e)
        {
            if (DataTransferManager.ActiveFileDownloads.Count == 1)
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
            ListedFileDownloads.Add(DataTransferManager.ActiveFileDownloads.LastOrDefault());
        }

        private async Task<bool> GetPeernetStatus()
        {
            var status = await statusService.GetStatus();

            if (status == null)
            {
                return false;
            }

            ConnectionStatus = status.IsConnected ? ConnectionStatus.Online : ConnectionStatus.Offline;
            Peers = status.CountPeerList;
            return status.IsConnected;
        }

        private async Task UpdateStatuses()
        {
            while (true)
            {
                var status = await statusService.GetStatus();
                if (status != null)
                {
                    Peers = status.CountPeerList;
                    PeerStatuses = new(await statusService.GetPeersStatus());
                }

                await Task.Delay(3000);
            }
        }

        private async Task UploadFiles()
        {
            var fileModels = applicationManager.OpenFileDialog().Select(f => new FileModel(f)).ToList();

            if (fileModels.Count != 0)
            {
                var parameter = new ShareFileViewModelParameter(DataTransferManager, warehouseClient, blockchainService, navigationService, notificationsManager, currentUserDirectoryViewModel)
                {
                    FileModels = fileModels
                };

                await modalNavigationService.Navigate<ShareFileViewModel, ShareFileViewModelParameter>(parameter);
            }
        }
    }
}