using Peernet.SDK.Models.Presentation.Profile;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    public interface IUserService
    {
        Task<User> GetUser(byte[] nodeId);
    }
}