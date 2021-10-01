using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface ICmdClient
    {
        /// <summary>
        /// Provides current connectivity status to the network
        /// </summary>
        /// <returns></returns>
        ApiResponseStatus GetStatus();

        /// <summary>
        /// Provides information about the self peer details
        /// </summary>
        /// <returns></returns>
        ApiResponsePeerSelf GetMyInfo();

        /// <summary>
        /// Add file to the blockchain
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        ApiBlockchainBlockStatus AddFiles(ApiBlockchainAddFiles files);

        /// <summary>
        /// Read a block of the blockchain
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        ApiBlockchainBlock ReadBlock(int block);

        /// <summary>
        /// Submit a search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SearchRequestResponse SubmitSearch(SearchRequest request);

        /// <summary>
        /// Return search results
        /// </summary>
        /// <param name="id">UUID</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        SearchResult ResturnSearch(string id, int? limit = null);

        /// <summary>
        /// Terminate a search
        /// </summary>
        /// <param name="id">UUID</param>
        void TerminateSearch(string id);
    }
}