using System.Threading.Tasks;
using MvvmCross.Navigation;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.ViewModels.Parameters
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

        public override string ModalTitle => "Edit File";

        public override bool ShouldUpdateFormat => false;

        public override async Task Confirm(FileModel[] files)
        {
            foreach (var fileModel in files)
            {
                var result = await blockchainService.UpdateFile(fileModel);
                if (result.Status != BlockchainStatus.StatusOK)
                {
                    GlobalContext.Notifications.Add(new Notification($"Failed to update the file. Status: {result.Status}", Severity.Error));
                }
            }

            await navigationService.Navigate<DirectoryViewModel>();
        }
    }
}