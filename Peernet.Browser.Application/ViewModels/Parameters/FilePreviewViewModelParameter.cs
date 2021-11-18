using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class FilePreviewViewModelParameter
    {
        public FilePreviewViewModelParameter(ApiFile file, bool isEditable,
            bool actionButtonEnabled, string actionButtonContent)
        {
            ActionButtonContent = actionButtonContent;
            ActionButtonEnabled = actionButtonEnabled;
            IsEditable = isEditable;
            File = file;
        }

        public string ActionButtonContent { get; set; }

        public bool ActionButtonEnabled { get; set; }

        public bool IsEditable { get; set; }

        public ApiFile File { get; set; }
    }
}