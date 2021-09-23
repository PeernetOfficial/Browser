using System.Collections.Generic;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public interface IVirtualFileSystemFactory
    {
        VirtualFileSystem CreateVirtualFileSystem(IEnumerable<ApiBlockRecordFile> sharedFiles);
    }
}