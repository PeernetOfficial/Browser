using System.Collections.Generic;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public interface IVirtualFileSystemFactory
    {
        VirtualFileSystem CreateVirtualFileSystem(IEnumerable<ApiBlockRecordFile> sharedFiles);
    }
}