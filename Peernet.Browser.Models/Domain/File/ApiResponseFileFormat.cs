using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Models.Domain.File
{
    public class ApiResponseFileFormat
    {
        public int Status { get; set; }

        public LowLevelFileType FileType { get; set; }

        public HighLevelFileType FileFormat { get; set; }
    }
}