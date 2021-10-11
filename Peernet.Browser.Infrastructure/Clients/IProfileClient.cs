using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;

namespace Peernet.Browser.Infrastructure.Clients
{
    public interface IProfileClient
    {
        Task<ApiBlockchainBlockStatus> AddUserImage(byte[] content);

        Task<ApiBlockchainBlockStatus> AddUserName(string userName);

        Task<ApiBlockchainBlockStatus> DeleteUserImage();

        Task<byte[]> GetUserImage();

        Task<string> GetUserName();
    }
}