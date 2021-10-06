using System.Collections.Generic;
using System.Threading.Tasks;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Facades
{
    public interface IBlockchainFacade
    {
        Task AddFilesAsync(IEnumerable<SharedNewFileModel> files);
    }
}