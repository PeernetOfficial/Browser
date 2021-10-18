using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public enum FileFormats
    {
        [Description("Binary File")]
        Binary,

        [Description("PDF File")]
        Pdf,

        [Description("Word File")]
        Word,

        [Description("Excel File")]
        Excel,

        [Description("Powerpoint File")]
        Powerpoint,

        [Description("Pictures")]
        Images,

        [Description("Audio file")]
        Audio,

        [Description("Movies")]
        Movies,

        [Description("Containers")]
        Container,

        [Description("Website")]
        WebSite,

        [Description("Text file")]
        Text,

        [Description("Ebooks file")]
        Ebook,

        [Description("Compressed file")]
        Compressed,

        [Description("Database file")]
        Database,

        [Description("Email file")]
        Email,

        [Description("CSV files")]
        CSV,

        [Description("Folders")]
        Folder
    }
}