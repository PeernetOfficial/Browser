using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Warehouse;
using Peernet.Browser.Models.Presentation.Footer;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using Peernet.Browser.Application.Utilities;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class ShareFileViewModelParameter : FileParameterModel
    {
        private readonly IWarehouseService warehouseService;
        private readonly IBlockchainService blockchainService;

        public ShareFileViewModelParameter(IWarehouseService warehouseService, IBlockchainService blockchainService)
        {
            this.warehouseService = warehouseService;
            this.blockchainService = blockchainService;
        }

        public override string ModalTitle => "Share File";

        public override bool ShouldUpdateFormat => true;

        public override async Task Confirm(FileModel[] files)
        {
            foreach (var file in files)
            {
                var warehouseResult = await warehouseService.Create(file);
                if (warehouseResult?.Status == WarehouseStatus.StatusOK)
                {
                    file.Hash = warehouseResult.Hash;
                }
                else
                {
                    GlobalContext.Notifications.Add(new Notification(
                        $"Failed to create warehouse. Status: {warehouseResult?.Status.ToString() ?? "[Unknown]"}",
                        MessagingHelper.GetInOutSummary(files, warehouseResult), Severity.Error));
                    return;
                }
            }

            var result = await blockchainService.AddFiles(files.Where(f => f.Hash != null));
            if (result.Status != BlockchainStatus.StatusOK)
            {
                GlobalContext.Notifications.Add(new Notification($"Failed to add files. Status: {result.Status}", MessagingHelper.GetInOutSummary(files, result), Severity.Error));
            }

            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<DirectoryViewModel>();
        }
    }
}