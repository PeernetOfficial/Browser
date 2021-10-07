using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    internal class ApiWrapper : WrapperBase, IApiWrapper
    {
        private readonly IHttpExecutor httpExecutor;

        public ApiWrapper(ISettingsManager settingsManager)
        {
            httpExecutor = new HttpExecutor(settingsManager);
        }

        public override string CoreSegment => string.Empty;

        public async Task<ApiResponseStatus> GetStatus()
        {
            return await httpExecutor.GetResult<ApiResponseStatus>(HttpMethod.Get, GetRelativeRequestPath("status"));
        }
    }
}