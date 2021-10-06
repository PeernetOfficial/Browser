using Peernet.Browser.Application.Helpers;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class SearchWrapper : WrapperBase
    {
        public SearchWrapper(IHttpClientFactory httpClientFactory)
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