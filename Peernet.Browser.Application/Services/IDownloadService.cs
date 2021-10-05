using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface IDownloadService
    {
        ApiResponseDownloadStatus Start(string path, string hash, string blockchain);

        ApiResponseDownloadStatus GetStatus(string hash, string blockchain);
    }
}
