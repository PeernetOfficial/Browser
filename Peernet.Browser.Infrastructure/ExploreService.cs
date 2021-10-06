using System.Collections.Generic;
using System.Net.Http;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System.Threading.Tasks;
using Peernet.Browser.Application.Helpers;

namespace Peernet.Browser.Infrastructure
{
    public class ExploreService : ServiceBase, IExploreService
    {
        public ExploreService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
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

            return await HttpHelper.GetResult<SearchResult>(HttpClient, HttpMethod.Get, GetRelativeRequestPath(string.Empty), parameters);
        }
    }
}