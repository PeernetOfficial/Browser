using Peernet.Browser.Models.Domain.Common;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public interface IVirtualFileSystemFactory
    {
        VirtualFileSystem CreateVirtualFileSystem(IEnumerable<ApiFile> sharedFiles, bool isCurrentSelection = true);
    }
}