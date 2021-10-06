using System.Threading.Tasks;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface IDownloadService
    {
        Task<ApiResponseDownloadStatus> Start(string path, byte[] hash, byte[] node);

        Task<ApiResponseDownloadStatus> GetStatus(string id);

        Task<ApiResponseDownloadStatus> GetAction(string id, DownloadAction action);
    }
}
