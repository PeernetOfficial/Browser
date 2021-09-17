using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public abstract class VirtualFileSystemEntity
    {
        protected VirtualFileSystemEntity(string name, List<ApiBlockRecordFile> files)
        {
            Name = name;
            Files = files;
        }

        public string Name { get; }

        public List<ApiBlockRecordFile> Files { get; }

        public abstract List<ApiBlockRecordFile> GetAllFiles();
    }
}