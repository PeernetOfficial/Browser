using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    internal interface IApiWrapper
    {
        /// <summary>
        /// Provides current connectivity status to the network
        /// </summary>
        /// <returns></returns>
        Task<ApiResponseStatus> GetStatus();
    }
}