using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using System.Threading.Tasks;
using Peernet.Browser.Application.Helpers;

namespace Peernet.Browser.Infrastructure
{
    public class SearchService : ServiceBase
    {
        public SearchService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        public override string CoreSegment => "search";

        public async Task<SearchResult> GetSearchResult(int id, int limit = 20)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id.ToString(),
                [nameof(limit)] = limit.ToString()
            };

            return await HttpHelper.GetResult<SearchResult>(HttpClient, HttpMethod.Get, GetRelativeRequestPath("result"), parameters);
        }

        public async Task<SearchRequestResponse> SubmitSearch(SearchRequest searchRequest)
        {
            return await HttpHelper.GetResult<SearchRequestResponse>(HttpClient, HttpMethod.Get, string.Empty, content: JsonContent.Create(searchRequest));
        }

        public async Task TerminateSearch(int id)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id.ToString(),
            };

            await HttpHelper.GetResult<SearchResult>(HttpClient, HttpMethod.Get, GetRelativeRequestPath("terminate"), parameters);
        }
    }
}