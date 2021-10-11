using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public enum FileFormats
    {
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
        Movies
    }
}