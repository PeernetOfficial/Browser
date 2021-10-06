namespace Peernet.Browser.Models.Domain
{
    public class Progress
    {
        public int TotalSize { get; set; }
        
        public int DownloadSize { get; set; }

        public double Percentage { get; set; }
    }
}
