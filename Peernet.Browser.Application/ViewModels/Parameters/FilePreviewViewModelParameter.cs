using Peernet.SDK.Models.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class FilePreviewViewModelParameter
    {
        public FilePreviewViewModelParameter(ApiFile file,
            Func<Task> action, string actionButtonContent)
        {
            ActionButtonContent = actionButtonContent;
            Action = action;
            File = file;
        }

        public Func<Task> Action { get; set; }
        public string ActionButtonContent { get; set; }
        public ApiFile File { get; set; }
    }
}