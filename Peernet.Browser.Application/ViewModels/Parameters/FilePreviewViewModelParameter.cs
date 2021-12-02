using System;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Common;

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

        public string ActionButtonContent { get; set; }

        public Func<Task> Action { get; set; }

        public bool IsEditable { get; set; }

        public ApiFile File { get; set; }
    }
}