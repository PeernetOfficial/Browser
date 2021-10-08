using Peernet.Browser.Models.Presentation;
using System.Threading.Tasks;
using Peernet.Browser.Models.Presentation.Profile;

namespace Peernet.Browser.Application.Facades
{
    public interface IProfileFacade
    {
        Task<User> GetUser();

        Task UpdateUser(string name, byte[] image);

        Task DeleteUserImage();
    }
}