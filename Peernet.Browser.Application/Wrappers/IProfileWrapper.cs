using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Wrappers
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