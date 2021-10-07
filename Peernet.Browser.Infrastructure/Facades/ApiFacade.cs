using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Wrappers;
using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Facades
{
    public class ApiFacade : IApiFacade
    {
        private readonly IApiWrapper apiWrapper;

        public ApiFacade(ISettingsManager settingsManager)
        {
            apiWrapper = new ApiWrapper(settingsManager);
        }

        public async Task<ApiResponseStatus> GetStatus()
        {
            return await apiWrapper.GetStatus();
        }
    }
}