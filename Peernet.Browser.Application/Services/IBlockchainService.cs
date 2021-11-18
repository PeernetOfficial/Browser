using System.Collections.Generic;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.Services
{
    public interface IBlockchainService
    {
        Task<ApiBlockchainBlockStatus> AddFiles(IEnumerable<FileModel> files);

        Task<ApiBlockchainBlockStatus> DeleteFile(ApiFile apiFile);

        Task<ApiBlockchainHeader> GetHeader();

        Task<List<ApiFile>> GetList();

        Task<ApiBlockchainBlockStatus> UpdateFile(FileModel fileModel);
    }
}