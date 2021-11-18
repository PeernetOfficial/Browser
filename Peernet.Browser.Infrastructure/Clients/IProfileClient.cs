using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Profile;

namespace Peernet.Browser.Infrastructure.Clients
{
    public interface IProfileClient
    {
        Task<ApiBlockchainBlockStatus> UpdateUser(string userName, byte[] image);

        Task<ApiBlockchainBlockStatus> DeleteUserImage();

        Task<ApiProfileData> GetProfileData();
    }
}