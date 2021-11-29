using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public enum FileFormat
    {
        [Description("None")]
        None = -1,

        [Description("Binary")]
        Binary = 0,

        [Description("PDF")]
        Pdf = 1,

        [Description("Word")]
        Word = 2,

        [Description("Excel")]
        Excel = 3,

        [Description("PowerPoint")]
        Powerpoint = 4,

        [Description("Picture")]
        Images = 5,

        [Description("Audio")]
        Audio = 6,

        [Description("Video")]
        Movies = 7,

        [Description("Container")]
        Container = 8,

        [Description("Website")]
        WebSite = 9,

        [Description("Text")]
        Text = 10,

        [Description("Ebook")]
        Ebook = 11,

        [Description("Compressed")]
        Compressed = 12,

        [Description("Database")]
        Database = 13,

        [Description("Email")]
        Email = 14,

        [Description("CSV")]
        CSV = 15,

        [Description("Folder")]
        Folder = 16,

        [Description("Executable")]
        Executable = 17,

        [Description("Installer")]
        Installer = 18,

        [Description("APK")]
        APK = 19,

        [Description("ISO")]
        ISO = 20
    }
}