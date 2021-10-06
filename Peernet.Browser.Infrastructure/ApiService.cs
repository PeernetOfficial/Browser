using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System.Net.Http;
using System.Threading.Tasks;
using Peernet.Browser.Application.Helpers;

namespace Peernet.Browser.Infrastructure
{
    public class ApiService : ServiceBase, IApiService
    {
        public ApiService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        public override string CoreSegment => string.Empty;

        public async Task<ApiResponseStatus> GetStatus()
        {
            return await HttpHelper.GetResult<ApiResponseStatus>(HttpClient, HttpMethod.Get, GetRelativeRequestPath("status"));
        }
    }
}