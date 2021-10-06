using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Wrappers;
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
        private readonly IDownloadWrapper downloadService;
        private readonly Timer timer;

        public DownloadManager(IDownloadWrapper downloadService)
        {
            this.downloadService = downloadService;
            timer = new Timer(async _ => await UpdateStatuses(), new AutoResetEvent(false), 1000, Timeout.Infinite);
        }

        public ObservableCollection<DownloadModel> ActiveFileDownloads { get; set; } = new();

        public async Task QueueUpDownload(ApiBlockRecordFile file)
        {
            var status = await downloadService.Start(@"C:/Temp", file.Hash, file.NodeId);

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
            return await downloadService.GetAction(id, DownloadAction.Pause);
        }

        public async Task<ApiResponseDownloadStatus> ResumeDownload(string id)
        {
            return await downloadService.GetAction(id, DownloadAction.Resume);
        }

        public async Task<ApiResponseDownloadStatus> CancelDownload(string id)
        {
            return await downloadService.GetAction(id, DownloadAction.Cancel);
        }

        private async Task UpdateStatuses()
        {
            foreach (var download in ActiveFileDownloads)
            {
                var status = await downloadService.GetStatus(download.Id);
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