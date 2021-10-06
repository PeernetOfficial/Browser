using System.Collections.Generic;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemCategory : VirtualFileSystemEntity
    {
        public VirtualFileSystemCategory(string category, VirtualFileSystemEntityType type, List<ApiBlockRecordFile> categoryFiles)
            : base(category, type, categoryFiles)
        {
        }
    }
}