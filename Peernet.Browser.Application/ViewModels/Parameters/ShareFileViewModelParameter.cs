using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Client.Http;
using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Presentation;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class ShareFileViewModelParameter : FileParameterModel
    {
        private readonly IDataTransferManager dataTransferManager;
        private readonly IWarehouseClient warehouseClient;
        private readonly IBlockchainService blockchainService;
        private readonly INavigationService navigationService;
        private readonly INotificationsManager notificationsManager;
        private readonly DirectoryViewModel directoryViewModel;

        public int Progress { get; set; }

        public ShareFileViewModelParameter(IDataTransferManager dataTransferManager, IWarehouseClient warehouseClient, IBlockchainService blockchainService, INavigationService navigationService, INotificationsManager notificationsManager, DirectoryViewModel directoryViewModel)
        {
            this.dataTransferManager = dataTransferManager;
            this.warehouseClient = warehouseClient;
            this.blockchainService = blockchainService;
            this.navigationService = navigationService;
            this.notificationsManager = notificationsManager;
            this.directoryViewModel = directoryViewModel;
        }

        public override string ModalTitle => "Share File";

        public override bool ShouldUpdateFormat => true;

        public override async Task Confirm(FileModel[] files)
        {
            foreach (var file in files)
            {
                var progress = new Progress<UploadProgress>();
                var upload = new Upload(warehouseClient, file, progress);
                await dataTransferManager.QueueUp(upload);

                if (file.Hash != null)
                {
                    var result = await blockchainService.AddFiles(new[] { file });
                    if (result.Status != BlockchainStatus.StatusOK)
                    {
                        var details =
                            MessagingHelper.GetApiSummary(
                                $"{nameof(blockchainService)}.{nameof(blockchainService.AddFiles)}") +
                            MessagingHelper.GetInOutSummary(file, result);
                        notificationsManager.Notifications.Add(new Notification($"Failed to add files. Status: {result.Status}", details, Severity.Error));
                    }

                    UIThreadDispatcher.ExecuteOnMainThread(async () => await directoryViewModel.ReloadVirtualFileSystem());
                }
            }
        }        
    }
}