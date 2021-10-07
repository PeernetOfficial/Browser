using System.Threading.Tasks;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    internal interface ISearchWrapper
    {
        Task<SearchResult> GetSearchResult(string id, int? limit = null);

        Task<SearchRequestResponse> SubmitSearch(SearchRequest searchRequest);

        Task TerminateSearch(string id);
    }
}