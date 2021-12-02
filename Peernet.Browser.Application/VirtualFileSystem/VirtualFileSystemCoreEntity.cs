using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemCoreEntity : VirtualFileSystemEntity
    {
        private bool isSelected;

        public VirtualFileSystemCoreEntity(string name, VirtualFileSystemEntityType type, string absolutePath)
        : base(null, name, type)
        {
            AbsolutePath = absolutePath;
        }

        public string AbsolutePath { get; }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public List<VirtualFileSystemEntity> VirtualFileSystemEntities { get; set; } = new();

        public VirtualFileSystemCoreEntity GetSelected()
        {
            if (IsSelected)
            {
                return this;
            }

            VirtualFileSystemCoreEntity selected = null;
            foreach (var virtualFileSystemEntity in VirtualFileSystemEntities)
            {
                if (virtualFileSystemEntity is VirtualFileSystemCoreEntity coreEntity)
                {
                    selected = coreEntity.GetSelected();
                    if (selected != null)
                    {
                        break;
                    }
                }
            }

            return selected;
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