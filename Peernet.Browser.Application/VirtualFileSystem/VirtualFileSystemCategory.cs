using Peernet.Browser.Models.Domain.Common;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemCategory : VirtualFileSystemEntity
    {
        public VirtualFileSystemCategory(string category, VirtualFileSystemEntityType type, List<ApiFile> categoryFiles)
            : base(category, type, categoryFiles)
        {
        }
    }
}