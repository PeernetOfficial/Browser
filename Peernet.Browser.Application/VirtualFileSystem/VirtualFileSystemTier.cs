using System.Collections.Generic;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemTier : VirtualFileSystemEntity
    {
        public VirtualFileSystemTier(string name, VirtualFileSystemEntityType type, int depth)
            : base(name, type, new List<ApiBlockRecordFile>())
        { 
            Depth = depth;
        }

        public int Depth { get; }

        public List<VirtualFileSystemTier> VirtualFileSystemTiers { get; set; } = new();

        public override List<ApiBlockRecordFile> GetAllFiles()
        {
            var files = new List<ApiBlockRecordFile>();

            var currentTierFiles = Files;
            if (currentTierFiles != null)
            {
                files.AddRange(currentTierFiles);
            }

            if (VirtualFileSystemTiers is { Count: > 0 })
            {
                foreach (var subTier in VirtualFileSystemTiers)
                {
                    files.AddRange(subTier.GetAllFiles());
                }
            }

            return files;
        }
    }
}
