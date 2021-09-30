using System.ComponentModel;

namespace Peernet.Browser.Application.Enums
{
    public enum FileFormats
    {
        [Description("All")]
        All,

        [Description("PDF File")]
        Pdf,

        [Description("Word File")]
        Word,

        [Description("Website")]
        WebSite,

        [Description("Excel File")]
        Excel,

        [Description("Powerpoint File")]
        Powerpoint,

        [Description("EPUB File")]
        Epub,

        [Description("Images")]
        Images,

        [Description("Movies")]
        Movies,
    }
}