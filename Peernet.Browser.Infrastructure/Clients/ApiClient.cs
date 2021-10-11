using System.Net.Http;
using System.Threading.Tasks;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class ApiClient : ClientBase, IApiClient
    {
        private readonly IHttpExecutor httpExecutor;

        public ApiClient(ISettingsManager settingsManager)
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