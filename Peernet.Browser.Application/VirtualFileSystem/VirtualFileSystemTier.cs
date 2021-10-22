using Peernet.Browser.Models.Domain.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemTier : VirtualFileSystemEntity
    {
        public VirtualFileSystemTier(string name, VirtualFileSystemEntityType type, int depth)
            : base(name, type, new List<ApiFile>())
        {
            Depth = depth;
        }

        public int Depth { get; }

        public ObservableCollection<VirtualFileSystemTier> VirtualFileSystemTiers { get; set; } = new();

        public override void ResetSelection()
        {
            VirtualFileSystemTiers.Foreach(t => t.ResetSelection());

            base.ResetSelection();
        }
    }
}