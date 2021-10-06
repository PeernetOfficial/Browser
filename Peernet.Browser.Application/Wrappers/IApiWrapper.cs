using System.Threading.Tasks;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Application.Services
{
    public interface IApiWrapper
    {
        /// <summary>
        /// Provides current connectivity status to the network
        /// </summary>
        /// <returns></returns>
        Task<ApiResponseStatus> GetStatus();
    }
}