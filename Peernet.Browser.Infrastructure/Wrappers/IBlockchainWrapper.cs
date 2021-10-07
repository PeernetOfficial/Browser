using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    internal interface IBlockchainWrapper
    {
        Task<ApiBlockchainHeader> GetSelfHeader();

        Task<ApiBlockchainAddFiles> GetSelfList();

        Task DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile);

        Task<ApiBlockchainBlockStatus> AddFiles(ApiBlockchainAddFiles files);

        Task<ApiBlockchainBlock> ReadBlock(int block);
    }
}