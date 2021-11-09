using Peernet.Browser.Models.Domain.Common;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemCoreCategory : VirtualFileSystemCoreEntity
    {
        public VirtualFileSystemCoreCategory(string category, VirtualFileSystemEntityType type, List<VirtualFileSystemEntity> categoryFiles)
            : base(category, type)
        {
            VirtualFileSystemEntities = categoryFiles;
        }
    }
}