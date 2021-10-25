using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Footer;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
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

        public override string ModalTitle => "Share file";

        public override async Task Confirm(FileModel[] files)
        {
            // There should be validation added all the way within this method
            foreach (var file in files)
            {
                var warehouseEntry = await warehouseService.Create(file);
                if (warehouseEntry.Status == 0)
                {
                    file.Hash = warehouseEntry.Hash;
                }
            }

            await blockchainService.AddFiles(files.Where(f => f.Hash != null));
        }
    }
}