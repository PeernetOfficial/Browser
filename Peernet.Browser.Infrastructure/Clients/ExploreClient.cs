using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Search;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class ExploreClient : ClientBase, IExploreClient
    {
        private readonly IHttpExecutor httpExecutor;

        public ExploreClient(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
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

            return await httpExecutor.GetResultAsync<SearchResult>(HttpMethod.Get, GetRelativeRequestPath(string.Empty), parameters);
        }
    }
}