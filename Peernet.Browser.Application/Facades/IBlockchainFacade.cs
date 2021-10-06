using Peernet.Browser.Models.Presentation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain;

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