using System.Threading.Tasks;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface IApiService
    {
        /// <summary>
        /// Provides current connectivity status to the network
        /// </summary>
        /// <returns></returns>
        Task<ApiResponseStatus> GetStatus();
    }
}