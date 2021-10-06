using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Peernet.Browser.Application.Helpers;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class ExploreWrapper : WrapperBase, IExploreWrapper
    {
        public ExploreWrapper(IHttpClientFactory httpClientFactory)
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