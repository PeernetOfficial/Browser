using Peernet.SDK.Models.Domain.Common;
using System;
using System.ComponentModel;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    [Serializable]
    public class VirtualFileSystemEntity : IEquatable<VirtualFileSystemEntity>, INotifyPropertyChanged
    {
        private readonly VirtualFileSystemEntityType? type;
        private string name;

        // For serialization
        public VirtualFileSystemEntity()
        {
        }

        public VirtualFileSystemEntity(ApiFile file, string name = null, VirtualFileSystemEntityType? type = null)
        {
            File = file;
            this.name = name;
            this.type = type;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public HighLevelFileType? DataFormat => File?.Format;
        public DateTime? Date => File?.Date;

        public ApiFile File { get; set; }

        public string FileSize => $"{File?.Size}";

        public string Folder => File?.Folder;
        public bool IsPlayerEnabled { get; set; }

        public string Name
        {
            get => name ?? File.Name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public VirtualFileSystemEntityType Type => GetEntityType();

        public bool Equals(VirtualFileSystemEntity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return name == other.name && type == other.type && Equals(File, other.File) && Date == other.Date;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((VirtualFileSystemEntity)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, type, File);
        }

        private VirtualFileSystemEntityType GetEntityType()
        {
            if (type != null)
            {
                return type.Value;
            }

            if (File == null)
            {
                return VirtualFileSystemEntityType.Directory;
            }

            return Enum.TryParse(File.Type.ToString(), true, out VirtualFileSystemEntityType entityType) ? entityType : VirtualFileSystemEntityType.Binary;
        }

        protected void RaiseEntityPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}