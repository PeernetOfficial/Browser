namespace Peernet.Browser.Models.Domain
{
    public enum DownloadStatus
    {
        DownloadWaitMetadata = 0,
        DownloadWaitSwarm = 1,
        DownloadActive = 2,
        DownloadPause = 3,
        DownloadCanceled = 4,
        DownloadFinished = 5
    }
}
