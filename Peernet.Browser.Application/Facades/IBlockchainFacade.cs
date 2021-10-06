using Peernet.Browser.Models.Presentation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Facades
{
    public interface IBlockchainFacade
    {
        Task AddFilesAsync(IEnumerable<SharedNewFileModel> files);
    }
}