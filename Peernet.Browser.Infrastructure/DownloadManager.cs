using System.Collections.ObjectModel;
using System.ComponentModel;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Infrastructure
{
    public class DownloadManager : INotifyPropertyChanged, IDownloadManager
    {
        private readonly IDownloadService downloadService;

        public DownloadManager(IDownloadService downloadService)
        {
            this.downloadService = downloadService;
        }

        public ObservableCollection<ApiBlockRecordFile> ActiveFileDownloads { get; set; } = new();

        public void QueueUpDownload(ApiBlockRecordFile file)
        {
            var status = downloadService.Start(@"C:/Temp", file.Hash, file.NodeId);

            if (status.Status == DownloadStatus.Success)
            {
                ActiveFileDownloads.Add(file);
            }
        }

        public void DequeueDownload(ApiBlockRecordFile file)
        {
            ActiveFileDownloads.Remove(file);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
