using System.Threading.Tasks;
using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Infrastructure.Facades
{
    public class ApiFacade : IApiFacade
    {
        private readonly IApiWrapper apiWrapper;

        public ApiFacade(IApiWrapper apiWrapper)
        {
            this.apiWrapper = apiWrapper;
        }

        public async Task<ApiResponseStatus> GetStatus()
        {
            return await apiWrapper.GetStatus();
        }
    }
}
