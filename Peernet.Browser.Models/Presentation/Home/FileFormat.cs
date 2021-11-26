using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public enum FileFormat
    {
        [Description("None")]
        None,

        [Description("Binary")]
        Binary,

        [Description("PDF")]
        Pdf,

        [Description("Word")]
        Word,

        [Description("Excel")]
        Excel,

        [Description("PowerPoint")]
        Powerpoint,

        [Description("Picture")]
        Images,

        [Description("Audio")]
        Audio,

        [Description("Video")]
        Movies,

        [Description("Container")]
        Container,

        [Description("Website")]
        WebSite,

        [Description("Text")]
        Text,

        [Description("Ebook")]
        Ebook,

        [Description("Compressed")]
        Compressed,

        [Description("Database")]
        Database,

        [Description("Email")]
        Email,

        [Description("CSV")]
        CSV,

        [Description("Folder")]
        Folder
    }
}