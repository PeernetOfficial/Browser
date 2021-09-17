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
    }
}