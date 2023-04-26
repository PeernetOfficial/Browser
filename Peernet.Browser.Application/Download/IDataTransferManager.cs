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

        Task PauseTransfer(Guid id);

        Task ResumeTransfer(Guid id);

        Task CancelTransfer(Guid id);

        void OpenFileLocation(string name);
    }
}