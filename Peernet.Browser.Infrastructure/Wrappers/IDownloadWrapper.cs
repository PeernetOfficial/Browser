using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public interface IDownloadWrapper
    {
        Task<ApiResponseDownloadStatus> Start(string path, byte[] hash, byte[] node);

        Task<ApiResponseDownloadStatus> GetStatus(string id);

        Task<ApiResponseDownloadStatus> GetAction(string id, DownloadAction action);
    }
}