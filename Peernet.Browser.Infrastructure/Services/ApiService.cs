using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Common;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class ApiService : IApiService
    {
        private readonly IApiClient apiClient;

        public ApiService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponseStatus> GetStatus()
        {
            return await apiClient.GetStatus();
        }
    }
}