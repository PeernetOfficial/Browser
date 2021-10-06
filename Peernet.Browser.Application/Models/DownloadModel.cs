namespace Peernet.Browser.Application.Models
{
    public record DownloadModel(string Id, ApiBlockRecordFile File)
    {
        public double Progress { get; set; }
    }
}
