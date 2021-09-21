using System.Collections.Generic;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public interface IFilesToCategoryBinder
    {
        List<VirtualFileSystemCategory> Bind(List<ApiBlockRecordFile> files);
    }
}