using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Utilities;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Domain.Download;
using Peernet.SDK.Models.Presentation;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    internal class DataTransferManager : IDataTransferManager
    {
        private readonly ISettingsManager settingsManager;
        private readonly INotificationsManager notificationsManager;

        public DataTransferManager(ISettingsManager settingsManager, INotificationsManager notificationsManager)

        {
            this.settingsManager = settingsManager;
            Directory.CreateDirectory(settingsManager.DownloadPath);
            this.notificationsManager = notificationsManager;

            // Fire on the thread-pool and forget
            Task.Run(UpdateStatuses);
        }

        public event EventHandler downloadsChanged;

        public ObservableCollection<DataTransfer> ActiveFileDownloads { get; set; } = new();

        public async Task<ApiResponseDownloadStatus> CancelTransfer(string id)
        {
            var dataTransfer = ActiveFileDownloads.First(d => d.Id == id);
            var responseStatus = await dataTransfer.Cancel();

            switch (responseStatus.DownloadStatus)
            {
                case DownloadStatus.DownloadCanceled:
                    ActiveFileDownloads.Remove(dataTransfer);
                    NotifyChange($"Data transfer canceled!");
                    break;

                case DownloadStatus.DownloadFinished:
                    ActiveFileDownloads.Remove(dataTransfer);
                    downloadsChanged?.Invoke(this, EventArgs.Empty);
                    break;
            }

            return responseStatus;
        }

        public void OpenFileLocation(string name)
        {
            var filePath = Path.Combine(settingsManager.DownloadPath, UtilityHelper.StripInvalidWindowsCharactersFromFileName(name));
            if (File.Exists(filePath))
            {
                Process.Start("explorer.exe", "/select, \"" + filePath + "\"");
            }
        }

        public async Task<ApiResponseDownloadStatus> PauseTransfer(string id)
        {
            var download = ActiveFileDownloads.First(d => d.Id == id);
            return await download.Pause();
        }

        public async Task QueueUp(DataTransfer dataTransfer)
        {
            dataTransfer.Completed += DataTransfer_Completed;
            UIThreadDispatcher.ExecuteOnMainThread(() =>
            {
                ActiveFileDownloads.Add(dataTransfer);
                downloadsChanged?.Invoke(this, EventArgs.Empty);
            });
            await dataTransfer.Start();
            //var strippedFileName = UtilityHelper.StripInvalidWindowsCharactersFromFileName(downloadModel.File.Name);
            //var status = await downloadClient.Start($"{settingsManager.DownloadPath}/{strippedFileName}", downloadModel.File.Hash, downloadModel.File.NodeId);


            //if (responseStatus.APIStatus == APIStatus.DownloadResponseSuccess)
            //{
            //    ActiveFileDownloads.Add(dataTransfer);
            //}
            //else
            //{
            //    //var details =
            //    //    MessagingHelper.GetApiSummary($"{nameof(downloadClient)}.{nameof(downloadClient.Start)}") +
            //    //    MessagingHelper.GetInOutSummary(dataTransfer.File, responseStatus);
            //    //notificationsManager.Notifications.Add(new Notification(
            //    //    $"Failed to start file download. Status: {responseStatus.APIStatus}", details, Severity.Error));
            //}
        }

        private void DataTransfer_Completed(object sender, EventArgs e)
        {
            var type = sender.GetType();
            UIThreadDispatcher.ExecuteOnMainThread(() =>
            {
                NotifyChange($"{(sender as DataTransfer)?.Name} {type.Name} completed!");
            });
        }

        public async Task<ApiResponseDownloadStatus> ResumeTransfer(string id)
        {
            var dataTransfer = ActiveFileDownloads.First(d => d.Id == id);
            return await dataTransfer.Resume();
        }

        //private async Task<ApiResponseDownloadStatus> ExecuteDownload(DownloadModel download, DownloadAction action)
        //{
        //    var responseStatus = await downloadClient.GetAction(download.Id, action);
        //    download.Status = responseStatus.DownloadStatus;
        //    downloadsChanged?.Invoke(this, EventArgs.Empty);

        //    if (responseStatus.APIStatus != APIStatus.DownloadResponseSuccess)
        //    {
        //        var details =
        //            MessagingHelper.GetApiSummary($"{nameof(downloadClient)}.{nameof(downloadClient.GetAction)}") +
        //            MessagingHelper.GetInOutSummary(download.Id, responseStatus);
        //        notificationsManager.Notifications.Add(new Notification(
        //            $"Failed to {action} file download. Status: {responseStatus.APIStatus}", details, Severity.Error));
        //    }

        //    return responseStatus;
        //}

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
                await Task.WhenAll(copy.Where(dataTransfer => !dataTransfer.IsCompleted).Select(dataTransfer => dataTransfer.UpdateStatus()));
                // Add 'Completed' event and in handler: UIThreadDispatcher.ExecuteOnMainThread(() => NotifyChange($"{download.File.Name} downloading completed!"));

                await Task.Delay(1000);
            }
        }
    }
}