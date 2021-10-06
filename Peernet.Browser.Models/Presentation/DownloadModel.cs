using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Models.Presentation
{
    public record DownloadModel(string Id, ApiBlockRecordFile File)
    {
        public double Progress { get; set; }
    }
}