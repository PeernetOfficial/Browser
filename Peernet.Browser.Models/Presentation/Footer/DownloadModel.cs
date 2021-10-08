using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Models.Presentation.Footer
{
    public record DownloadModel(string Id, ApiBlockRecordFile File)
    {
        public double Progress { get; set; }
    }
}