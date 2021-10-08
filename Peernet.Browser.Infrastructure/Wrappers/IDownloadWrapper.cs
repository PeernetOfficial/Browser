using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Download;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public interface IDownloadWrapper
    {
        Task<ApiResponseDownloadStatus> Start(string path, byte[] hash, byte[] node);

        Task<ApiResponseDownloadStatus> GetStatus(string id);

        Task<ApiResponseDownloadStatus> GetAction(string id, DownloadAction action);
    }
}