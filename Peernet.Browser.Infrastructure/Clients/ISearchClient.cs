using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Search;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface ISearchClient
    {
        Task<SearchResult> GetSearchResult(string id, int? limit = null);

        Task<SearchRequestResponse> SubmitSearch(SearchRequest searchRequest);

        Task TerminateSearch(string id);
    }
}