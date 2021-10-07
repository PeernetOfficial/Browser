using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Wrappers;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Presentation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class DownloadManager : INotifyPropertyChanged, IDownloadManager, IDisposable
    {
        private readonly IDownloadWrapper _downloadWrapper;
        private readonly Timer timer;

        public DownloadManager(ISettingsManager settingsManager)
        {
            _downloadWrapper = new DownloadWrapper(settingsManager);
            timer = new Timer(
                async _ => await GlobalContext.UiThreadDispatcher.ExecuteOnMainThreadAsync(async () =>
                    await UpdateStatuses()), new AutoResetEvent(false), 1000, Timeout.Infinite);
        }

        // TODO: It needs to be both concurrent and observable collection
        public ObservableCollection<DownloadModel> ActiveFileDownloads { get; set; } = new();

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

        public async Task DequeueDownload(string id)
        {
            var downloadToDequeue = ActiveFileDownloads.First(d => d.Id == id);
            ActiveFileDownloads.Remove(downloadToDequeue);
        }

        public async Task<ApiResponseDownloadStatus> PauseDownload(string id)
        {
            return await _downloadWrapper.GetAction(id, DownloadAction.Pause);
        }

        public async Task<ApiResponseDownloadStatus> ResumeDownload(string id)
        {
            return await _downloadWrapper.GetAction(id, DownloadAction.Resume);
        }

        public async Task<ApiResponseDownloadStatus> CancelDownload(string id)
        {
            return await _downloadWrapper.GetAction(id, DownloadAction.Cancel);
        }

        private async Task UpdateStatuses()
        {
            foreach (var download in ActiveFileDownloads)
            {
                var status = await _downloadWrapper.GetStatus(download.Id);
                download.Progress = status.Progress.Percentage;

                if (status.DownloadStatus == DownloadStatus.DownloadFinished)
                {
                    ActiveFileDownloads.Remove(download);
                }
            }

            timer.Change(1000, Timeout.Infinite);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}