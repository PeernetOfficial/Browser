using System.Threading.Tasks;
using Peernet.Browser.Models.Presentation.Profile;

namespace Peernet.Browser.Application.Services
{
    public interface IProfileService
    {
        Task<User> GetUser();

        Task UpdateUser(string name, byte[] image);

        Task DeleteUserImage();
    }
}