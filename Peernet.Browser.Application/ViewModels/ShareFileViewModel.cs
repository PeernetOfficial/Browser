using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Client.Clients;

namespace Peernet.Browser.Application.ViewModels
{
    // To enable generic type expression in XAML
    public class ShareFileViewModel : GenericFileViewModel<ShareFileViewModelParameter>
    {
        public ShareFileViewModel(
            IDataTransferManager dataTransferManager,
            INavigationService navigationService,
            IModalNavigationService modalNavigationService,
            IApplicationManager applicationManager,
            IBlockchainService blockchainService,
            IWarehouseClient warehouseClient,
            IFileService fileService,
            INotificationsManager notificationsManager,
            DirectoryViewModel directoryViewModel)
            : base(
                  dataTransferManager,
                  navigationService,
                  modalNavigationService,
                  applicationManager,
                  blockchainService,
                  warehouseClient,
                  fileService,
                  notificationsManager,
                  directoryViewModel)
        {
        }
    }
}