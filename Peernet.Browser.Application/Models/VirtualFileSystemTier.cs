using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class VirtualFileSystemTier : VirtualFileSystemEntity
    {
        public VirtualFileSystemTier(string name, int depth)
            : base(name, new List<ApiBlockRecordFile>())
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
