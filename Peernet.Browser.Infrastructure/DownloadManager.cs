using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Download;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    internal class DownloadManager : IDownloadManager
    {
        private readonly IDownloadClient downloadClient;
        private readonly ISettingsManager settingsManager;
        private readonly INotificationsManager notificationsManager;
        private readonly IUIThreadDispatcher uiThreadDispatcher;

        public DownloadManager(IDownloadClient downloadClient, ISettingsManager settingsManager, INotificationsManager notificationsManager, IUIThreadDispatcher uIThreadDispatcher)

        {
            this.settingsManager = settingsManager;
            Directory.CreateDirectory(settingsManager.DownloadPath);
            this.downloadClient = downloadClient;
            this.notificationsManager = notificationsManager;
            this.uiThreadDispatcher = uIThreadDispatcher;

            // Fire on the thread-pool and forget
            Task.Run(UpdateStatuses);
        }

        public event EventHandler downloadsChanged;

        public ObservableCollection<DownloadModel> ActiveFileDownloads { get; set; } = new();

        public async Task<ApiResponseDownloadStatus> CancelDownload(string id)
        {
            var download = ActiveFileDownloads.First(d => d.Id == id);
            var responseStatus = await ExecuteDownload(download, DownloadAction.Cancel);

            switch (responseStatus.DownloadStatus)
            {
                case DownloadStatus.DownloadCanceled:
                    ActiveFileDownloads.Remove(download);
                    NotifyChange($"{download.File.Name} downloading canceled!");
                    break;

                case DownloadStatus.DownloadFinished:
                    ActiveFileDownloads.Remove(download);
                    downloadsChanged?.Invoke(this, EventArgs.Empty);
                    break;
            }

            return responseStatus;
        }

        public void OpenFileLocation(string name)
        {
            var filePath = Path.Combine(settingsManager.DownloadPath, name);
            if (File.Exists(filePath))
            {
                Process.Start("explorer.exe", "/select, " + filePath);
            }
        }

        public async Task<ApiResponseDownloadStatus> PauseDownload(string id)
        {
            var download = ActiveFileDownloads.First(d => d.Id == id);
            return await ExecuteDownload(download, DownloadAction.Pause);
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
            else
            {
                var details =
                    MessagingHelper.GetApiSummary($"{nameof(downloadClient)}.{nameof(downloadClient.Start)}") +
                    MessagingHelper.GetInOutSummary(downloadModel.File, status);
                notificationsManager.Notifications.Add(new Notification(
                    $"Failed to start file download. Status: {status.APIStatus}", details, Severity.Error));
            }

            downloadsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<ApiResponseDownloadStatus> ResumeDownload(string id)
        {
            var download = ActiveFileDownloads.First(d => d.Id == id);
            return await ExecuteDownload(download, DownloadAction.Resume);
        }

        private async Task<ApiResponseDownloadStatus> ExecuteDownload(DownloadModel download, DownloadAction action)
        {
            var responseStatus = await downloadClient.GetAction(download.Id, action);
            download.Status = responseStatus.DownloadStatus;
            downloadsChanged?.Invoke(this, EventArgs.Empty);

            if (responseStatus.APIStatus != APIStatus.DownloadResponseSuccess)
            {
                var details =
                    MessagingHelper.GetApiSummary($"{nameof(downloadClient)}.{nameof(downloadClient.GetAction)}") +
                    MessagingHelper.GetInOutSummary(download.Id, responseStatus);
                notificationsManager.Notifications.Add(new Notification(
                    $"Failed to {action} file download. Status: {responseStatus.APIStatus}", details, Severity.Error));
            }

            return responseStatus;
        }

        private void NotifyChange(string message)
        {
            downloadsChanged?.Invoke(this, EventArgs.Empty);
            notificationsManager.Notifications.Add(new Notification(message));
        }

        private async Task UpdateStatuses()
        {
            while (true)
            {
                var copy = ActiveFileDownloads.ToList();
                foreach (var download in copy.Where(download => !download.IsCompleted))
                {
                    var status = await downloadClient.GetStatus(download.Id);
                    if (status == null)
                    {
                        continue;
                    }

                    download.Progress = status.Progress.Percentage;
                    download.Status = status.DownloadStatus;

                    if (status.DownloadStatus == DownloadStatus.DownloadFinished)
                    {
                        download.IsCompleted = true;
                        download.Progress = 100;
                        uiThreadDispatcher.ExecuteOnMainThread(() =>
                            NotifyChange($"{download.File.Name} downloading completed!"));
                    }
                }

                await Task.Delay(1000);
            }
        }
    }
}