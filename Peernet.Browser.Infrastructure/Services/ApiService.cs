using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Common;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    public class ApiService : IApiService
    {
        private readonly IApiClient apiClient;

        public ApiService(ISettingsManager settingsManager)
        {
            apiClient = new ApiClient(settingsManager);
        }

        public async Task<ApiResponseStatus> GetStatus()
        {
            return await apiClient.GetStatus();
        }
    }
}