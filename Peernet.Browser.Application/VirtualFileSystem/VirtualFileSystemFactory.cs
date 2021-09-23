using Peernet.Browser.Application.Models;
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

        public VirtualFileSystem CreateVirtualFileSystem(IEnumerable<ApiBlockRecordFile> sharedFiles)
        {
            return new VirtualFileSystem(sharedFiles, binder);
        }
    }
}