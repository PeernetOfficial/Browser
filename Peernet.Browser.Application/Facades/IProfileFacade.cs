using Peernet.Browser.Models.Presentation;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Facades
{
    public interface IProfileFacade
    {
        Task<User> GetUser();

        Task UpdateUser(string name, byte[] image);

        Task DeleteUserImage();
    }
}