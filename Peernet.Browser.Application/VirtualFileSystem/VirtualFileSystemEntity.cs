using System.Collections.Generic;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public abstract class VirtualFileSystemEntity
    {
        protected VirtualFileSystemEntity(string name, VirtualFileSystemEntityType type, List<ApiBlockRecordFile> files)
        {
            Name = name;
            Type = type;
            Files = files;
        }

        public string Name { get; }

        public VirtualFileSystemEntityType Type { get; }

        public List<ApiBlockRecordFile> Files { get; }

        public abstract List<ApiBlockRecordFile> GetAllFiles();
    }
}