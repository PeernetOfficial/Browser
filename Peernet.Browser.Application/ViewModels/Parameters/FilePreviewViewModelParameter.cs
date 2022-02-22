using Peernet.SDK.Models.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class FilePreviewViewModelParameter
    {
        public FilePreviewViewModelParameter(ApiFile file, bool isEditable,
            Func<Task> action, string actionButtonContent)
        {
            ActionButtonContent = actionButtonContent;
            Action = action;
            IsEditable = isEditable;
            File = file;
        }

        public Func<Task> Action { get; set; }
        public string ActionButtonContent { get; set; }
        public ApiFile File { get; set; }
        public bool IsEditable { get; set; }
    }
}