using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditFileViewModelParameter : FileParameterModel
    {
        private readonly IBlockchainService blockchainService;

        public EditFileViewModelParameter(IBlockchainService blockchainService)
        {
            this.blockchainService = blockchainService;
        }

        public override async Task Confirm(FileModel[] files)
        {
            foreach (var fileModel in files)
            {
                await blockchainService.UpdateFile(fileModel);
            }
        }
    }
}