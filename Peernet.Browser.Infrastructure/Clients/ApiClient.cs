using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Common;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class ApiClient : ClientBase, IApiClient
    {
        private readonly IHttpExecutor httpExecutor;

        public ApiClient(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
        }

        public override string CoreSegment => string.Empty;

        public async Task<ApiResponseStatus> GetStatus()
        {
            return await httpExecutor.GetResultAsync<ApiResponseStatus>(HttpMethod.Get, GetRelativeRequestPath("status"));
        }
    }
}