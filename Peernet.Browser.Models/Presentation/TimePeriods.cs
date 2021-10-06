using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation
{
    public enum TimePeriods
    {
        [Description("Last 24 hours")]
        Last24,

        [Description("Last Week")]
        LastWeek,

        [Description("Last Month")]
        LastMounth,

        [Description("Last Year")]
        LastYear,

        [Description("Custom")]
        Custom
    }
}