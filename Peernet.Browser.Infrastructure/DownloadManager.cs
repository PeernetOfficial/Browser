using System.Collections.Generic;
using System.Collections.ObjectModel;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Wrappers;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Download;
using Peernet.Browser.Models.Presentation.Footer;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var downloadToDequeue = ActiveFileDownloads.First(d => d.Id == id);
            var responseStatus = await _downloadWrapper.GetAction(id, DownloadAction.Cancel);
            if (responseStatus.DownloadStatus is DownloadStatus.DownloadCanceled or DownloadStatus.DownloadFinished)
            {
                ActiveFileDownloads.Remove(downloadToDequeue);
            }

            return responseStatus;
        }

        public async Task<ApiResponseDownloadStatus> PauseDownload(string id)
        {
            var responseStatus = await _downloadWrapper.GetAction(id, DownloadAction.Pause);
            ActiveFileDownloads.First(d => d.Id == id).Status = responseStatus.DownloadStatus;

            return responseStatus;
        }

        public async Task QueueUpDownload(ApiBlockRecordFile file)
        {
            var status = await _downloadWrapper.Start($"C:/Temp/{file.Name}", file.Hash, file.NodeId);
            var downloadModel = new DownloadModel(status.Id, file)
            {
                Status = status.DownloadStatus
            };

            if (status.APIStatus == APIStatus.DownloadResponseSuccess)
            {
                ActiveFileDownloads.Add(downloadModel);
            }

            if (status.APIStatus == APIStatus.DownloadResponseFileInvalid)
            {
                // This is just for testing, whole condition should be handled in proper way once clear how.
                ActiveFileDownloads.Add(downloadModel);
            }
        }

        public async Task<ApiResponseDownloadStatus> ResumeDownload(string id)
        {
            var responseStatus = await _downloadWrapper.GetAction(id, DownloadAction.Resume);

            ActiveFileDownloads.First(d => d.Id == id).Status = responseStatus.DownloadStatus;

            return responseStatus;
        }

        private async Task UpdateStatuses()
        {
            while (true)
            {
                // It should enumerate copy - not actual collection so the exception is not thrown when modifying collection from other thread
                foreach (var download in ActiveFileDownloads)
                {
                    var status = await _downloadWrapper.GetStatus(download.Id);
                    download.Progress = status.Progress.Percentage;
                    download.Status = status.DownloadStatus;

                    if (status.DownloadStatus == DownloadStatus.DownloadFinished)
                    {
                        download.Progress = 100;
                        // To preserve thread-affinity
                        // This causes updating to stop. When item is removed from collection the exception is thrown (modified during enumeration) and method exits
                        await GlobalContext.UiThreadDispatcher.ExecuteOnMainThreadAsync(() => ActiveFileDownloads.Remove(download));
                    }
                }

                Thread.Sleep(3000);
            }
        }
    }
}