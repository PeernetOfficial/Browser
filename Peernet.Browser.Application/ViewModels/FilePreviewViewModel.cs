using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Application.ViewModels
{
    public class FilePreviewViewModel : ViewModelBase<FilePreviewViewModelParameter>
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
            set => SetProperty(ref actionButtonContent, value);
        }

        public IMvxAsyncCommand DownloadCommand => new MvxAsyncCommand(
            async () =>
            {
                await ButtonAction();
            });

        public ApiFile File { get; set; }

        public bool IsEditable
        {
            get => isEditable;
            set => SetProperty(ref isEditable, value);
        }

        public override void Prepare(FilePreviewViewModelParameter parameter)
        {
            File = parameter.File;
            IsEditable = parameter.IsEditable;
            ButtonAction = parameter.Action;
            ActionButtonContent = parameter.ActionButtonContent;
        }
    }
}