using System.Threading.Tasks;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Application.Wrappers
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