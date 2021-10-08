using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Wrappers;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Presentation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Download;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Infrastructure
{
    public class DownloadManager : IDownloadManager
    {
        private readonly IDownloadWrapper _downloadWrapper;

        public DownloadManager(ISettingsManager settingsManager)
        {
            _downloadWrapper = new DownloadWrapper(settingsManager);

            // Fire on the thread-pool and forget
            Task.Run(UpdateStatuses);
        }

        public ObservableCollection<DownloadModel> ActiveFileDownloads { get; set; } = new();

        public async Task<ApiResponseDownloadStatus> CancelDownload(string id)
        {
            return await _downloadWrapper.GetAction(id, DownloadAction.Cancel);
        }

        public async Task DequeueDownload(string id)
        {
            var downloadToDequeue = ActiveFileDownloads.First(d => d.Id == id);
            ActiveFileDownloads.Remove(downloadToDequeue);
        }

        public async Task<ApiResponseDownloadStatus> PauseDownload(string id)
        {
            return await _downloadWrapper.GetAction(id, DownloadAction.Pause);
        }

        public async Task QueueUpDownload(ApiBlockRecordFile file)
        {
            var status = await _downloadWrapper.Start($"C:/Temp/{file.Name}", file.Hash, file.NodeId);

            if (status.APIStatus == APIStatus.DownloadResponseSuccess)
            {
                ActiveFileDownloads.Add(new(status.Id, file));
            }

            if (status.APIStatus == APIStatus.DownloadResponseFileInvalid)
            {
                // This is just for testing, whole condition should be handled in proper way once clear how.
                ActiveFileDownloads.Add(new(status.Id, file));
            }
        }

        public async Task<ApiResponseDownloadStatus> ResumeDownload(string id)
        {
            return await _downloadWrapper.GetAction(id, DownloadAction.Resume);
        }

        private async Task UpdateStatuses()
        {
            while (true)
            {
                foreach (var download in ActiveFileDownloads)
                {
                    var status = await _downloadWrapper.GetStatus(download.Id);
                    download.Progress = status.Progress.Percentage;

                    if (status.DownloadStatus == DownloadStatus.DownloadFinished)
                    {
                        // To preserve thread-affinity
                        await GlobalContext.UiThreadDispatcher.ExecuteOnMainThreadAsync(() => ActiveFileDownloads.Remove(download));
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}