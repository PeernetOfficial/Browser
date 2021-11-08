using MvvmCross.ViewModels;
using Peernet.Browser.Models.Domain.Common;
using System;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemEntity : MvxNotifyPropertyChanged
    {
        private readonly string name;
        private readonly VirtualFileSystemEntityType? type;

        public VirtualFileSystemEntity(ApiFile file, string name = null, VirtualFileSystemEntityType? type = null)
        {
            File = file;
            this.name = name;
            this.type = type;
        }

        public string Name => name ?? File.Name;

        public DateTime? Date => File?.Date;

        public VirtualFileSystemEntityType Type => type ?? Enum.Parse<VirtualFileSystemEntityType>(File.Type.ToString());

        public ApiFile File { get; init; }
    }
}