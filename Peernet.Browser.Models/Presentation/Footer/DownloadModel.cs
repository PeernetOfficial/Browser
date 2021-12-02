using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Download;
using Peernet.Browser.Models.Presentation.Home;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Peernet.Browser.Models.Presentation.Footer
{
    public class DownloadModel : INotifyPropertyChanged
    {
        private bool isCompleted;
        private bool isMapEnabled;
        private double progress;
        private DownloadStatus status;

        public DownloadModel(ApiFile file)
        {
            File = file;
            Points = Map.ConvertGeoPointsToMapScale(file.SharedByGeoIP);
            IsMapEnabled = !Points.IsNullOrEmpty();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static MapModel Map { get; } = new() { Width = 471, Height = 231 };
        public string DisplayName => File.Name.Length > 26 ? $"{File.Name.Substring(0, 26)}..." : File.Name;
        public ApiFile File { get; init; }
        public string FileSize => $"{File.Size}";
        public string Id { get; set; }

        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }

        public bool IsMapEnabled
        {
            get => isMapEnabled;
            set
            {
                isMapEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMapEnabled)));
            }
        }

        public List<GeoPoint> Points { get; }

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
    }
}