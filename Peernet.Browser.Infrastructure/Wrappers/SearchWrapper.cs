using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class SearchWrapper : WrapperBase, ISearchWrapper
    {
        private readonly IHttpExecutor httpExecutor;

        public SearchWrapper(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
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

            await httpExecutor.GetResult<SearchResult>(HttpMethod.Get, GetRelativeRequestPath("terminate"), parameters);
        }
    }
}