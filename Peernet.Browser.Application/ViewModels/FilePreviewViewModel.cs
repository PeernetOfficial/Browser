using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Models.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class FilePreviewViewModel : GenericViewModelBase<FilePreviewViewModelParameter>
    {
        private string actionButtonContent;
        private Func<Task> ButtonAction;
        private bool isEditable;

        public FilePreviewViewModel()
        {
        }

        public string ActionButtonContent
        {
            get => actionButtonContent;
            set
            {
                actionButtonContent = value;
                OnPropertyChanged(nameof(ActionButtonContent));
            }
        }

        public IAsyncCommand DownloadCommand => new AsyncCommand(
            async () =>
            {
                await ButtonAction();
            });

        public ApiFile File { get; set; }

        public bool IsEditable
        {
            get => isEditable;
            set
            {
                isEditable = value;
                OnPropertyChanged(nameof(IsEditable));
            }
        }

        public override Task Prepare(FilePreviewViewModelParameter parameter)
        {
            Parameter = parameter;
            File = Parameter.File;
            IsEditable = Parameter.IsEditable;
            ButtonAction = Parameter.Action;
            ActionButtonContent = Parameter.ActionButtonContent;

            return Task.CompletedTask;
        }
    }
}