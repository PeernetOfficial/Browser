namespace Peernet.Browser.Models.Domain.Download
{
    public class Progress
    {
        public int TotalSize { get; set; }

        public int DownloadSize { get; set; }

        public double Percentage { get; set; }
    }
}