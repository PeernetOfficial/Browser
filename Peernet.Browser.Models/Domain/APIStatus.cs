namespace Peernet.Browser.Models.Domain
{
    public enum APIStatus
    {
        DownloadResponseSuccess = 0,
        DownloadResponseIDNotFound = 1,
        DownloadResponseFileInvalid = 2,
        DownloadResponseActionInvalid = 3,
        DownloadResponseFileWrite = 4
    }
}
