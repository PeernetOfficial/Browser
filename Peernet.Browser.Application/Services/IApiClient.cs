using Peernet.Browser.Application.Models;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IApiClient
    {
        Task<Status> GetStatus();

        Task<MyInfo> GetMyInfo();
    }
}