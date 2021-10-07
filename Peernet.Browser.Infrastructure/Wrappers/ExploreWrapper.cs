using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class ExploreWrapper : WrapperBase, IExploreWrapper
    {
        private readonly IHttpExecutor httpExecutor;

        public ExploreWrapper(IHttpClientFactory factory)
        {
            this.httpExecutor = new HttpExecutor(factory);
        }

        public override string CoreSegment => "explore";

        public async Task<SearchResult> GetFiles(int limit, int? type = null)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(limit)] = limit.ToString()
            };

            if (type != null)
            {
                parameters.Add("type", type.ToString());
            }

            return await httpExecutor.GetResult<SearchResult>(HttpMethod.Get, GetRelativeRequestPath(string.Empty), parameters);
        }
    }
}