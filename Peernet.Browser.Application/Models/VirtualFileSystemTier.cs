using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class VirtualFileSystemTier
    {
        public VirtualFileSystemTier(string name, int depth)
        {
            Name = name;
            Depth = depth;
        }

        public string Name { get; }

        public int Depth { get; }

        public List<ApiBlockRecordFile> Files { get; set; } = new();

        public List<VirtualFileSystemTier> VirtualFileSystemTiers { get; set; } = new();
    }
}
