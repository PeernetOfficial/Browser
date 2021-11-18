using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.ViewModels
{
    public class FilePreviewViewModel : MvxViewModel<FilePreviewViewModelParameter>
    {
        private readonly IDownloadManager downloadManager;
        private string actionButtonContent;
        private bool actionButtonEnabled;
        private bool isEditable;

        public FilePreviewViewModel(IDownloadManager downloadManager)
        {
            this.downloadManager = downloadManager;
        }

        public string ActionButtonContent
        {
            get => actionButtonContent;
            set => SetProperty(ref actionButtonContent, value);
        }

        public bool ActionButtonEnabled
        {
            get => actionButtonEnabled;
            set => SetProperty(ref actionButtonEnabled, value);
        }

        public IMvxAsyncCommand<ApiFile> DownloadCommand => new MvxAsyncCommand<ApiFile>(
            async file =>
            {
                await downloadManager.QueueUpDownload(new DownloadModel(file));
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
            ActionButtonContent = parameter.ActionButtonContent;
            ActionButtonEnabled = parameter.ActionButtonEnabled;
        }
    }
}