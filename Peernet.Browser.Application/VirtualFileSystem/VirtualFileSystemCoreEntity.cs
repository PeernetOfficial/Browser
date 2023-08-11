using System;
using System.Collections.ObjectModel;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    [Serializable]
    public class VirtualFileSystemCoreEntity : VirtualFileSystemEntity
    {
        private bool isSelected;

        public VirtualFileSystemCoreEntity()
            : base()
        {
        }

        public VirtualFileSystemCoreEntity(string name, VirtualFileSystemEntityType type, string path = null)
        : base(null, name, type)
        {
            Path = path ?? string.Empty;
        }

        public string Path { get; set; }

        public string AbsolutePath => System.IO.Path.Combine(Path, Name);

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                RaiseEntityPropertyChanged(nameof(IsSelected));
            }
        }

        public ObservableCollection<VirtualFileSystemEntity> VirtualFileSystemEntities { get; set; } = new();

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