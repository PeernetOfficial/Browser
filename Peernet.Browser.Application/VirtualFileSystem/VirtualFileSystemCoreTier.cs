using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemCoreTier : VirtualFileSystemCoreEntity
    {
        public VirtualFileSystemCoreTier(string name, VirtualFileSystemEntityType type, string path = null)
            : base(name, type, path)
        {
        }

        public List<VirtualFileSystemCoreTier> SubTiers => VirtualFileSystemEntities.OfType<VirtualFileSystemCoreTier>().ToList();
    }
}