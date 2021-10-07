using Peernet.Browser.Application.Http;
using Peernet.Browser.Models.Domain;
using System.Net.Http;
using System.Threading.Tasks;
using Peernet.Browser.Application.Wrappers;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class ApiWrapper : WrapperBase, IApiWrapper
    {
        private readonly IHttpExecutor httpExecutor;

        public ApiWrapper(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
        }

        public override string CoreSegment => string.Empty;

        public async Task<ApiResponseStatus> GetStatus()
        {
            return await httpExecutor.GetResult<ApiResponseStatus>(HttpMethod.Get, GetRelativeRequestPath("status"));
        }
    }
}