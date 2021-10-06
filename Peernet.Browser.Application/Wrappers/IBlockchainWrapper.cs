using System.Threading.Tasks;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Application.Wrappers
{
    public interface IBlockchainWrapper
    {
        Task<ApiBlockchainHeader> GetSelfHeader();

        Task<ApiBlockchainAddFiles> GetSelfList();

        Task DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile);

        Task<ApiBlockchainBlockStatus> AddFiles(ApiBlockchainAddFiles files);

        Task<ApiBlockchainBlock> ReadBlock(int block);
    }
}