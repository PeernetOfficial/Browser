using Peernet.Browser.Models.Domain.Common;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IApiClient
    {
        /// <summary>
        /// Provides current connectivity status to the network
        /// </summary>
        /// <returns></returns>
        Task<ApiResponseStatus> GetStatus();
    }
}