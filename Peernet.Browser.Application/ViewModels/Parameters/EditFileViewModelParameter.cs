using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.Threading.Tasks;
using Peernet.Browser.Application.Utilities;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class EditFileViewModelParameter : FileParameterModel
    {
        private readonly IBlockchainService blockchainService;
        private readonly Func<Task> postAction;

        public EditFileViewModelParameter(IBlockchainService blockchainService, Func<Task> postAction)
        {
            this.blockchainService = blockchainService;
            this.postAction = postAction;
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
                    var message = "Failed to update the file. Status: {result.Status}";
                    var details =
                        MessagingHelper.GetApiSummary(
                            $"{nameof(blockchainService)}.{nameof(blockchainService.UpdateFile)}") +
                        MessagingHelper.GetInOutSummary(files, result);
                    GlobalContext.Notifications.Add(new Notification(message, details, Severity.Error));
                }
            }

            if (postAction != null)
            {
                await postAction.Invoke();
            }
        }

    }
}