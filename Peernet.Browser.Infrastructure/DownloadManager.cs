using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Download;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class DownloadManager : IDownloadManager
    {
        private readonly IDownloadClient downloadClient;
        private readonly ISettingsManager settingsManager;

        public event EventHandler downloadsChanged;

        public DownloadManager(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            downloadClient = new DownloadClient(settingsManager);

            // Fire on the thread-pool and forget
            Task.Run(UpdateStatuses);
        }

        public ObservableCollection<DownloadModel> ActiveFileDownloads { get; set; } = new();

        public async Task<ApiResponseDownloadStatus> CancelDownload(string id)
        {
            var download = ActiveFileDownloads.First(d => d.Id == id);
            var responseStatus = await downloadClient.GetAction(id, DownloadAction.Cancel);
            if (responseStatus.DownloadStatus is DownloadStatus.DownloadCanceled or DownloadStatus.DownloadFinished)
            {
                ActiveFileDownloads.Remove(download);
                NotifyChange($"{download.File.Name} downloading canceled!");
            }

            return responseStatus;
        }

        public async Task<ApiResponseDownloadStatus> PauseDownload(string id)
        {
            var responseStatus = await downloadClient.GetAction(id, DownloadAction.Pause);
            ActiveFileDownloads.First(d => d.Id == id).Status = responseStatus.DownloadStatus;
            downloadsChanged?.Invoke(this, EventArgs.Empty);

            return responseStatus;
        }

        public async Task QueueUpDownload(DownloadModel downloadModel)
        {
            var status = await downloadClient.Start($"{settingsManager.DownloadPath}/{downloadModel.File.Name}", downloadModel.File.Hash, downloadModel.File.NodeId);
            downloadModel.Id = status.Id;
            downloadModel.Status = status.DownloadStatus;

            if (status.APIStatus == APIStatus.DownloadResponseSuccess)
            {
                ActiveFileDownloads.Add(downloadModel);
            }

            if (status.APIStatus == APIStatus.DownloadResponseFileInvalid)
            {
                // This is just for testing, whole condition should be handled in proper way once clear how.
                ActiveFileDownloads.Add(downloadModel);
            }

            downloadsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<ApiResponseDownloadStatus> ResumeDownload(string id)
        {
            var responseStatus = await downloadClient.GetAction(id, DownloadAction.Resume);
            ActiveFileDownloads.First(d => d.Id == id).Status = responseStatus.DownloadStatus;
            downloadsChanged?.Invoke(this, EventArgs.Empty);

            return responseStatus;
        }

        private void NotifyChange(string message)
        {
            downloadsChanged?.Invoke(this, EventArgs.Empty);
            GlobalContext.Notifications.Add(new Notification { Text = message });
        }

        private async Task UpdateStatuses()
        {
            while (true)
            {
                var copy = ActiveFileDownloads.ToList();
                foreach (var download in copy.Where(download => !download.IsCompleted))
                {
                    var status = await downloadClient.GetStatus(download.Id);
                    download.Progress = status.Progress.Percentage;
                    download.Status = status.DownloadStatus;

                    if (status.DownloadStatus == DownloadStatus.DownloadFinished)
                    {
                        download.IsCompleted = true;
                        download.Progress = 100;
                        await GlobalContext.UiThreadDispatcher.ExecuteOnMainThreadAsync(() =>
                            NotifyChange($"{download.File.Name} downloading completed!"));
                    }
                }

                await Task.Delay(1000);
            }
        }
    }
}