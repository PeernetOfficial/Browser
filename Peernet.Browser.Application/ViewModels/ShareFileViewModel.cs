using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;

namespace Peernet.Browser.Application.ViewModels
{
    // To enable generic type expression in XAML
    public class ShareFileViewModel : GenericFileViewModel<ShareFileViewModelParameter>
    {
        public ShareFileViewModel(
            INavigationService navigationService,
            IModalNavigationService modalNavigationService,
            IApplicationManager applicationManager,
            IBlockchainService blockchainService,
            IWarehouseService warehouseService,
            IFileService fileService,
            INotificationsManager notificationsManager,
            DirectoryViewModel directoryViewModel)
            : base(
                  navigationService,
                  modalNavigationService,
                  applicationManager,
                  blockchainService,
                  warehouseService,
                  fileService,
                  notificationsManager,
                  directoryViewModel)
        {
        }
    }
}
