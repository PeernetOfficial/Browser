using System;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    [Serializable]
    public class VirtualFileSystemCoreCategory : VirtualFileSystemCoreEntity
    {
        public VirtualFileSystemCoreCategory()
            : base()
        {
        }

        public VirtualFileSystemCoreCategory(string category, VirtualFileSystemEntityType type, List<VirtualFileSystemEntity> categoryFiles)
            : base(category, type)
        {
            VirtualFileSystemEntities = new(categoryFiles);
        }
    }
}