using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface ICmdClient
    {
        /// <summary>
        /// Provides current connectivity status to the network
        /// </summary>
        /// <returns></returns>
        Status GetStatus();

        /// <summary>
        /// Provides information about the self peer details
        /// </summary>
        /// <returns></returns>
        MyInfo GetMyInfo();

        /// <summary>
        /// Add file to the blockchain
        /// </summary>
        /// <param name="files"></param>
        void AddFiles(ApiBlockRecordFile[] files);
    }
}