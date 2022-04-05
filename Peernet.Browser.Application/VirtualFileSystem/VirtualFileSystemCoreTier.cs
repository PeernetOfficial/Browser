using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    [Serializable]
    public class VirtualFileSystemCoreTier : VirtualFileSystemCoreEntity
    {
        public VirtualFileSystemCoreTier()
            : base()
        {
        }

        public VirtualFileSystemCoreTier(string name, VirtualFileSystemEntityType type, string path = null)
            : base(name, type, path)
        {
        }

        public List<VirtualFileSystemCoreTier> SubTiers => VirtualFileSystemEntities.OfType<VirtualFileSystemCoreTier>().ToList();
    }
}