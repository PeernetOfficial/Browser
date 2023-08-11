using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Presentation.Profile;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IProfileService
    {
        Task<User> GetUser(byte[]? node = null);

        Task<ApiBlockchainBlockStatus> UpdateUser(string name, byte[] image);

        Task DeleteUserImage();
    }
}