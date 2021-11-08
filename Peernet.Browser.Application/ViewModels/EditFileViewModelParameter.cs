using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditFileViewModelParameter : FileParameterModel
    {
        private readonly IBlockchainService blockchainService;
        private readonly IApplicationManager applicationManager;

        public EditFileViewModelParameter(IBlockchainService blockchainService, IApplicationManager applicationManager)
        {
            this.blockchainService = blockchainService;
            this.applicationManager = applicationManager;
        }

        public override string ModalTitle => "Edit File";

        public override async Task Confirm(FileModel[] files)
        {
            foreach (var fileModel in files)
            {
                await blockchainService.UpdateFile(fileModel);
            }

            applicationManager.NavigateToMain(ViewType.Directory);
        }
    }
}