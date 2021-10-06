using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Presentation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Download
{
    public interface IDownloadManager
    {
        ObservableCollection<DownloadModel> ActiveFileDownloads { get; set; }

        Task QueueUpDownload(ApiBlockRecordFile file);

        Task DequeueDownload(string id);

        Task<ApiResponseDownloadStatus> PauseDownload(string id);

        Task<ApiResponseDownloadStatus> ResumeDownload(string id);

        Task<ApiResponseDownloadStatus> CancelDownload(string id);
    }
}