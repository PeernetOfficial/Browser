using Peernet.Browser.Application.Helpers;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Domain;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class ApiWrapper : WrapperBase, IApiWrapper
    {
        public ApiWrapper(IHttpClientFactory httpClientFactory)
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