using Peernet.Browser.Models.Domain;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public interface IVirtualFileSystemFactory
    {
        VirtualFileSystem CreateVirtualFileSystem(IEnumerable<ApiBlockRecordFile> sharedFiles);
    }
}