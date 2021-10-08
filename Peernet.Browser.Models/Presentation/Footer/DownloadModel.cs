using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Download;
using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Footer
{
    public class DownloadModel : INotifyPropertyChanged
    {
        private DownloadStatus status;
        private double progress;

        public DownloadModel(string Id, ApiBlockRecordFile File)
        {
            this.Id = Id;
            this.File = File;
        }

        public double Progress
        {
            get => progress;
            set
            {
                progress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
            }
        }

        public DownloadStatus Status
        {
            get => status;
            set
            {
                status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }

        public string Id { get; init; }

        public ApiBlockRecordFile File { get; init; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}