using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class EditFileViewModelParameter : FileParameterModel
    {
        private readonly IBlockchainService blockchainService;
        private readonly INotificationsManager notificationsManager;
        private readonly Func<Task> postAction;

        public EditFileViewModelParameter(IBlockchainService blockchainService, INotificationsManager notificationsManager, Func<Task> postAction)
        {
            this.blockchainService = blockchainService;
            this.notificationsManager = notificationsManager;
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
                    notificationsManager.Notifications.Add(new Notification(message, details, Severity.Error));
                }
            }

            if (postAction != null)
            {
                await postAction.Invoke();
            }
        }
    }
}