using Peernet.SDK.Models.Domain.Download;
using Peernet.SDK.Models.Presentation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Download
{
    public interface IDataTransferManager
    {
        event EventHandler downloadsChanged;

        ObservableCollection<DataTransfer> ActiveFileDownloads { get; set; }

        Task QueueUp(DataTransfer dataTransfer);

        Task PauseTransfer(string id);

        Task ResumeTransfer(string id);

        Task CancelTransfer(string id);

        void OpenFileLocation(string name);
    }
}