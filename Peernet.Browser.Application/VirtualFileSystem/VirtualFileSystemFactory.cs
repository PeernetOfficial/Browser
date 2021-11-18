using Peernet.Browser.Models.Domain.Common;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemFactory : IVirtualFileSystemFactory
    {
        private readonly IFilesToCategoryBinder binder;

        public VirtualFileSystemFactory(IFilesToCategoryBinder binder)
        {
            this.binder = binder;
        }

        public VirtualFileSystem CreateVirtualFileSystem(IEnumerable<ApiFile> sharedFiles, bool isCurrentSelection = true)
        {
            return new VirtualFileSystem(sharedFiles, binder, isCurrentSelection);
        }
    }
}