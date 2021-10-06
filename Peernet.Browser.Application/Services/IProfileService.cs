using Peernet.Browser.Application.Models;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IProfileService
    {
        Task<ApiBlockchainBlockStatus> AddUserImage(byte[] content);

        Task<ApiBlockchainBlockStatus> AddUserName(string userName);

        Task<ApiBlockchainBlockStatus> DeleteUserImage();

        Task<byte[]> GetUserImage();

        Task<string> GetUserName();
    }
}