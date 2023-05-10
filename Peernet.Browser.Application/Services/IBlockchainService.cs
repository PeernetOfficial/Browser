using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Domain.Search;
using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IBlockchainService
    {
        Task<ApiBlockchainBlockStatus> AddFiles(IEnumerable<FileModel> files);

        Task<ApiBlockchainBlockStatus> DeleteFile(ApiFile apiFile);

        Task<ApiBlockchainHeader> GetHeader();

        Task<List<ApiFile>> GetList();

        Task<ApiBlockchainBlockStatus> UpdateFile(FileModel fileModel);

        Task<SearchResult> GetFilesForNode(byte[] node);
    }
}