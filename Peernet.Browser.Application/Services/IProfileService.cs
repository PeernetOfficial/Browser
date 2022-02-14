using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Presentation.Profile;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IProfileService
    {
        Task<User> GetUser();

        Task<ApiBlockchainBlockStatus> UpdateUser(string name, byte[] image);

        Task DeleteUserImage();
    }
}