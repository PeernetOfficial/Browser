using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class SearchResult
    {
        public int Status { get; set; }

        public List<ApiBlockRecordFile> Files { get; set; }
    }
}
