using Peernet.SDK.Models.Domain.Common;
using System;
using System.ComponentModel;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemEntity : IEquatable<VirtualFileSystemEntity>, INotifyPropertyChanged
    {
        private readonly VirtualFileSystemEntityType? type;
        private string name;

        public VirtualFileSystemEntity(ApiFile file, string name = null, VirtualFileSystemEntityType? type = null)
        {
            File = file;
            this.name = name ?? File.Name;
            this.type = type;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public HighLevelFileType? DataFormat => File?.Format;
        public DateTime? Date => File?.Date;

        public ApiFile File { get; init; }

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

            return Enum.TryParse(File.Type.ToString(), true, out VirtualFileSystemEntityType entityType) ? entityType : VirtualFileSystemEntityType.Binary;
        }
    }
}