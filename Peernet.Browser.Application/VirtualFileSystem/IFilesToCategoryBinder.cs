using Peernet.Browser.Models.Domain.Common;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public interface IFilesToCategoryBinder
    {
        List<VirtualFileSystemCoreCategory> Bind(IEnumerable<ApiFile> files);
    }
}