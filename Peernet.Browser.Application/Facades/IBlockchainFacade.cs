using Peernet.Browser.Models.Presentation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.Facades
{
    public interface IBlockchainFacade
    {
        Task AddFilesAsync(IEnumerable<SharedNewFileModel> files);
        Task DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile);
        Task<ApiBlockchainHeader> GetSelfHeader();
        Task<List<ApiBlockRecordFile>> GetSelfList();
    }
}