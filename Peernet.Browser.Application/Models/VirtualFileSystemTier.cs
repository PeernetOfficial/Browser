using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Models
{
    public class VirtualFileSystemTier
    {
        public VirtualFileSystemTier(string name, int depth)
        {
            FileSystemTiers = new List<VirtualFileSystemTier>();
            Name = name;
            Depth = depth;
        }

        public string Name { get; }

        public int Depth { get; }

        public ApiBlockchainAddFiles Files { get; set; }

        public List<VirtualFileSystemTier> FileSystemTiers { get; set; }
    }
}
