using System.Collections.Generic;
using Peernet.Browser.Application.Models;

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