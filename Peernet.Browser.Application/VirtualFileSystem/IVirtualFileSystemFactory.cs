using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public interface IVirtualFileSystemFactory
    {
        VirtualFileSystem CreateVirtualFileSystem(IEnumerable<ApiFile> sharedFiles, bool isCurrentSelection = true);
    }
}