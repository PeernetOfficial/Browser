using Peernet.Browser.Models.Domain;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public interface IFilesToCategoryBinder
    {
        List<VirtualFileSystemCategory> Bind(IEnumerable<ApiBlockRecordFile> files);
    }
}