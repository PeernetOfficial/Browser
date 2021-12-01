using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemCoreEntity : VirtualFileSystemEntity
    {
        private bool isSelected;

        public VirtualFileSystemCoreEntity(string name, VirtualFileSystemEntityType type)
        : base(null, name, type)
        {
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                SetProperty(ref isSelected, value);
            }
        }

        public List<VirtualFileSystemEntity> VirtualFileSystemEntities { get; set; } = new();

        public VirtualFileSystemCoreEntity GetSelected()
        {
            if (IsSelected)
            {
                return this;
            }

            foreach (var virtualFileSystemEntity in VirtualFileSystemEntities)
            {
                if (virtualFileSystemEntity is VirtualFileSystemCoreEntity coreEntity)
                {
                    var selected = coreEntity.GetSelected();
                    return selected;
                }
            }

            return null;
        }

        public virtual void ResetSelection()
        {
            IsSelected = false;
            if (VirtualFileSystemEntities != null)
            {
                foreach (var entity in VirtualFileSystemEntities)
                {
                    if (entity is VirtualFileSystemCoreEntity coreEntity)
                    {
                        coreEntity.ResetSelection();
                    }
                }
            }
        }
    }
}