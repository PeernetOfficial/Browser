using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public interface IProfileWrapper
    {
        Task<ApiBlockchainBlockStatus> AddUserImage(byte[] content);

        Task<ApiBlockchainBlockStatus> AddUserName(string userName);

        Task<ApiBlockchainBlockStatus> DeleteUserImage();

        Task<byte[]> GetUserImage();

        Task<string> GetUserName();
    }
}