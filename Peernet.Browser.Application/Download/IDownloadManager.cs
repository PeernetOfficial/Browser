using Peernet.SDK.Models.Domain.Download;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Download
{
    public interface IDownloadManager
    {
        event EventHandler downloadsChanged;

        ObservableCollection<DownloadModel> ActiveFileDownloads { get; set; }

        Task QueueUpDownload(DownloadModel downloadModel);

        Task<ApiResponseDownloadStatus> PauseDownload(string id);

        Task<ApiResponseDownloadStatus> ResumeDownload(string id);

        Task<ApiResponseDownloadStatus> CancelDownload(string id);

        void OpenFileLocation(string name);
    }
}