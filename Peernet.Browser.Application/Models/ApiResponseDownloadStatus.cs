namespace Peernet.Browser.Application.Models
{
    public class ApiResponseDownloadStatus
    {
        public string Id { get; set; }

        public APIStatus APIStatus { get; set; }

        public DownloadStatus DownloadStatus { get; set; }

        public ApiBlockRecordFile File { get; set; }

        public Progress Progress { get; set; }

        public Swarm Swarm { get; set; }
    }
}
