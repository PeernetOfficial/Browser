using Peernet.Browser.Application.Enums;

namespace Peernet.Browser.Application.Models
{
    public class FileFormatFilterModel : CustomFilterModel<FileFormats>
    {
        public FileFormatFilterModel() : base("File format")
        {
        }
    }
}