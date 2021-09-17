using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class VirtualFileSystemCategory : VirtualFileSystemEntity
    {
        public VirtualFileSystemCategory(string category, List<ApiBlockRecordFile> categoryFiles)
            : base(category, categoryFiles)
        {
        }

        public override List<ApiBlockRecordFile> GetAllFiles() => Files;
    }
}