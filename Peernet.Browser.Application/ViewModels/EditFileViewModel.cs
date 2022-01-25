using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditFileViewModel : GenericFileViewModel<EditFileViewModelParameter>
    {
        public EditFileViewModel(
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
