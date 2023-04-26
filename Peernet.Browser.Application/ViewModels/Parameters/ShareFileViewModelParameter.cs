using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Presentation;
using Peernet.SDK.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class ShareFileViewModelParameter : FileParameterModel
    {
        private readonly IBlockchainService blockchainService;
        private readonly IDataTransferManager dataTransferManager;
        private readonly INavigationService navigationService;
        private readonly INotificationsManager notificationsManager;
        private readonly IWarehouseClient warehouseClient;
        private readonly DirectoryTabViewModel directoryTabViewModel;

        public ShareFileViewModelParameter(IDataTransferManager dataTransferManager, IWarehouseClient warehouseClient, IBlockchainService blockchainService, INavigationService navigationService, INotificationsManager notificationsManager, DirectoryTabViewModel directoryTabViewModel)
        {
            this.dataTransferManager = dataTransferManager;
            this.warehouseClient = warehouseClient;
            this.blockchainService = blockchainService;
            this.navigationService = navigationService;
            this.notificationsManager = notificationsManager;
            this.directoryTabViewModel = directoryTabViewModel;
        }

        public override string ModalTitle => "Share File";
        public int Progress { get; set; }
        public override bool ShouldUpdateFormat => true;

        public override async Task Confirm(FileModel[] files)
        {
            foreach (var file in files)
            {
                var upload = new Upload(warehouseClient, file);
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

                    UIThreadDispatcher.ExecuteOnMainThread(async () => await directoryTabViewModel.ReloadVirtualFileSystem());
                }
            }
        }
    }
}