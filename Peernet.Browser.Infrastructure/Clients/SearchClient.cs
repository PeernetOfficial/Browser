using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Search;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
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

        public async Task<SearchResult> GetSearchResult(SearchGetRequest searchGetRequest)
        {
            return await httpExecutor.GetResult<SearchResult>(HttpMethod.Get, GetRelativeRequestPath("result"), GetParams(searchGetRequest));
        }

        public async Task<SearchStatistic> SearchResultStatistics(string id)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id
            };
            return await httpExecutor.GetResult<SearchStatistic>(HttpMethod.Get, GetRelativeRequestPath("statistic"), parameters);
        }

        public async Task<SearchRequestResponse> SubmitSearch(SearchRequest searchRequest)
        {
            return await httpExecutor.GetResult<SearchRequestResponse>(HttpMethod.Post, GetRelativeRequestPath(string.Empty), content: JsonContent.Create(searchRequest));
        }

        public async Task<string> TerminateSearch(string id)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id,
            };

            return await httpExecutor.GetResult<string>(HttpMethod.Get, GetRelativeRequestPath("terminate"), parameters);
        }

        private Dictionary<string, string> GetParams(object obj)
        {
            var res = new Dictionary<string, string>();
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                var val = pi.GetValue(obj, null);
                if (val != null)
                {
                    res.Add(pi.Name.ToLower(), val.ToString());
                }
            }
            return res;
        }
    }
}