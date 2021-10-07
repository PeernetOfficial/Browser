using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Wrappers
{
    public interface ISearchWrapper
    {
        Task<SearchResult> GetSearchResult(string id, int? limit = null);

        Task<SearchRequestResponse> SubmitSearch(SearchRequest searchRequest);

        Task TerminateSearch(string id);
    }
}