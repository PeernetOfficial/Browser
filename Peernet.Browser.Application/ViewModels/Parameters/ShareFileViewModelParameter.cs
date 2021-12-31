using MvvmCross;
using MvvmCross.Navigation;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Warehouse;
using Peernet.Browser.Models.Presentation.Footer;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class ShareFileViewModelParameter : FileParameterModel
    {
        private readonly IBlockchainService blockchainService;
        private readonly IWarehouseService warehouseService;

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
                    var details =
                        MessagingHelper.GetApiSummary(
                            $"{nameof(warehouseService)}.{nameof(warehouseService.Create)}") +
                        MessagingHelper.GetInOutSummary(files, warehouseResult);
                    GlobalContext.Notifications.Add(new Notification(
                        $"Failed to create warehouse. Status: {warehouseResult?.Status.ToString() ?? "[Unknown]"}",
                        details, Severity.Error));
                    return;
                }
            }

            var result = await blockchainService.AddFiles(files.Where(f => f.Hash != null));
            if (result.Status != BlockchainStatus.StatusOK)
            {
                var details =
                    MessagingHelper.GetApiSummary(
                        $"{nameof(blockchainService)}.{nameof(blockchainService.AddFiles)}") +
                    MessagingHelper.GetInOutSummary(files, result);
                GlobalContext.Notifications.Add(new Notification($"Failed to add files. Status: {result.Status}", details, Severity.Error));
            }

            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<DirectoryViewModel>();
        }
    }
}