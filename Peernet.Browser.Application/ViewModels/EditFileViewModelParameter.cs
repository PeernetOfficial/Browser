using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Footer;
using System.Threading.Tasks;
using MvvmCross.Navigation;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditFileViewModelParameter : FileParameterModel
    {
        private readonly IBlockchainService blockchainService;
        private readonly IMvxNavigationService navigationService;

        public EditFileViewModelParameter(IBlockchainService blockchainService, IMvxNavigationService navigationService)
        {
            this.blockchainService = blockchainService;
            this.navigationService = navigationService;
        }

        public override string ModalTitle => "Edit file";

        public override async Task Confirm(FileModel[] files)
        {
            foreach (var fileModel in files)
            {
                await blockchainService.UpdateFile(fileModel);
            }

            await navigationService.Navigate<DirectoryViewModel>();
        }
    }
}