using System.Threading.Tasks;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Presentation;

namespace Peernet.Browser.Application.Facades
{
    public interface IProfileFacade
    {
        Task<User> GetUser();

        Task UpdateUser(string name, byte[] image);

        Task DeleteUserImage();
    }
}