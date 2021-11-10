using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IBlockchainClient
    {
        Task<ApiBlockchainHeader> GetHeader();

        Task<ApiBlockchainAddFiles> GetList();

        Task<ApiBlockchainBlockStatus> DeleteFile(ApiFile apiFile);

        Task<ApiBlockchainBlockStatus> AddFiles(ApiBlockchainAddFiles files);

        Task<ApiBlockchainBlock> ReadBlock(int block);

        Task<ApiBlockchainBlockStatus> UpdateFile(ApiFile apiFile);
    }
}