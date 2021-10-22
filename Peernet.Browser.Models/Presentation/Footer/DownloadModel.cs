using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Download;
using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Footer
{
    public class DownloadModel : INotifyPropertyChanged
    {
        private DownloadStatus status;
        private double progress;
        private bool isCompleted;

        public DownloadModel(ApiFile file)
        {
            File = file;
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

        public string DisplayName => File.Name.Length > 26 ? $"{File.Name.Substring(0, 26)}..." : File.Name;

        public string Id { get; set; }

        public ApiFile File { get; init; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }
    }
}