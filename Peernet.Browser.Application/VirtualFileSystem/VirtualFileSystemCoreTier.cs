using System.Collections.ObjectModel;
using System.Linq;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemCoreTier : VirtualFileSystemCoreEntity
    {
        public VirtualFileSystemCoreTier(string name, VirtualFileSystemEntityType type)
            : base(name, type)
        {
        }
    }
}