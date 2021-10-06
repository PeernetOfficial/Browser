using Peernet.Browser.Application.Models;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IBlockchainService
    {
        Task<ApiBlockchainHeader> GetSelfHeader();

        Task<ApiBlockchainAddFiles> GetSelfList();

        Task DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile);

        Task<ApiBlockchainBlockStatus> AddFiles(ApiBlockchainAddFiles files);

        Task<ApiBlockchainBlock> ReadBlock(int block);
    }
}