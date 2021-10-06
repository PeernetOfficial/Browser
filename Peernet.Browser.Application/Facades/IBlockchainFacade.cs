using System.Collections.Generic;
using System.Threading.Tasks;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Presentation;

namespace Peernet.Browser.Application.Facades
{
    public interface IBlockchainFacade
    {
        Task AddFilesAsync(IEnumerable<SharedNewFileModel> files);
    }
}