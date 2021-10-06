using Peernet.Browser.Application.Helpers;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Peernet.Browser.Application.Wrappers;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class SearchWrapper : WrapperBase, ISearchWrapper
    {
        public SearchWrapper(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        public override string CoreSegment => "search";

        public async Task<SearchResult> GetSearchResult(string id, int limit = 20)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id,
                [nameof(limit)] = limit.ToString()
            };

            return await HttpHelper.GetResult<SearchResult>(HttpClient, HttpMethod.Get, GetRelativeRequestPath("result"), parameters);
        }

        public async Task<SearchRequestResponse> SubmitSearch(SearchRequest searchRequest)
        {
            return await HttpHelper.GetResult<SearchRequestResponse>(HttpClient, HttpMethod.Post, GetRelativeRequestPath(string.Empty), content: JsonContent.Create(searchRequest));
        }

        public async Task TerminateSearch(string id)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id,
            };

            await HttpHelper.GetResult<SearchResult>(HttpClient, HttpMethod.Get, GetRelativeRequestPath("terminate"), parameters);
        }
    }
}