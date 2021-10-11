using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Search;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class SearchClient : ClientBase, ISearchClient
    {
        private readonly IHttpExecutor httpExecutor;

        public SearchClient(ISettingsManager settingsManager)
        {
            httpExecutor = new HttpExecutor(settingsManager);
        }

        public override string CoreSegment => "search";

        public async Task<SearchResult> GetSearchResult(string id, int? limit = null)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id
            };

            if (limit != null)
            {
                parameters.Add(nameof(limit), limit.ToString());
            }

            return await httpExecutor.GetResult<SearchResult>(HttpMethod.Get, GetRelativeRequestPath("result"), parameters);
        }

        public async Task<SearchRequestResponse> SubmitSearch(SearchRequest searchRequest)
        {
            return await httpExecutor.GetResult<SearchRequestResponse>(HttpMethod.Post, GetRelativeRequestPath(string.Empty), content: JsonContent.Create(searchRequest));
        }

        public async Task TerminateSearch(string id)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id,
            };

            await httpExecutor.GetResult<string>(HttpMethod.Get, GetRelativeRequestPath("terminate"), parameters);
        }
    }
}