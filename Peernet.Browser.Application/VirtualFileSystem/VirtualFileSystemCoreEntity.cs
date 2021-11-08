using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemCoreEntity : VirtualFileSystemEntity
    {
        private bool isVisualTreeVertex;
        private bool isSelected;

        public VirtualFileSystemCoreEntity(string name, VirtualFileSystemEntityType type)
        : base(null, name, type)
        {
        }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public bool IsVisualTreeVertex
        {
            get => isVisualTreeVertex;
            set => SetProperty(ref isVisualTreeVertex, value);
        }

        public List<VirtualFileSystemEntity> VirtualFileSystemEntities { get; set; } = new();

        public void ResetSelection()
        {
            VirtualFileSystemEntities.Foreach(t => t.ResetSelection());
        }
    }
}