using Peernet.Browser.Models.Domain.Search;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface ISearchClient
    {
        /// <summary>
        /// This function returns search results. The default limit is 100.
        /// </summary>
        /// <param name="searchGetRequest"></param>
        /// <returns></returns>
        Task<SearchResult> GetSearchResult(SearchGetRequest searchGetRequest);

        /// <summary>
        /// This starts a search request and returns an ID that can be used to collect the results asynchronously. Note that some of the filters described below (such as filetype) must be set to -1 if they are not used.
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        Task<SearchRequestResponse> SubmitSearch(SearchRequest searchRequest);

        /// <summary>
        /// The user can terminate a search early using this function. This helps save system resources and should be considered best practice once a search is no longer needed (for example when the user closes the tab or window that shows the results).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> TerminateSearch(string id);

        /// <summary>
        /// This returns search result statistics. Statistics are always calculated over all results, regardless of any applied runtime filters.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SearchStatistic> SearchResultStatistics(string id);
    }
}