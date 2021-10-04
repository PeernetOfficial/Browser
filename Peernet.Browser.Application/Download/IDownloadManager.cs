using System.Collections.Generic;
using System.Collections.ObjectModel;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Download
{
    public interface IDownloadManager
    {
        ObservableCollection<ApiBlockRecordFile> ActiveFileDownloads { get; set; }

        void QueueUpDownload(ApiBlockRecordFile file);

        void DequeueDownload(ApiBlockRecordFile file);
    }
}
