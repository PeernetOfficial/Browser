using Peernet.Browser.Models.Domain.Download;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    public interface IDownloadClient
    {
        Task<ApiResponseDownloadStatus> Start(string path, byte[] hash, byte[] node);

        Task<ApiResponseDownloadStatus> GetStatus(string id);

        Task<ApiResponseDownloadStatus> GetAction(string id, DownloadAction action);
    }
}