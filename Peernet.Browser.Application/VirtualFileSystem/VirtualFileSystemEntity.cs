using MvvmCross.ViewModels;
using Peernet.Browser.Models.Domain.Common;
using System;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemEntity : MvxNotifyPropertyChanged, IEquatable<VirtualFileSystemEntity>
    {
        private readonly string name;
        private readonly VirtualFileSystemEntityType? type;

        public VirtualFileSystemEntity(ApiFile file, string name = null, VirtualFileSystemEntityType? type = null)
        {
            File = file;
            this.name = name;
            this.type = type;
        }

        public DateTime? Date => File?.Date;

        public ApiFile File { get; init; }

        public string Name => name ?? File.Name;

        public VirtualFileSystemEntityType Type => type ?? Enum.Parse<VirtualFileSystemEntityType>(File.Type.ToString());

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
    }
}