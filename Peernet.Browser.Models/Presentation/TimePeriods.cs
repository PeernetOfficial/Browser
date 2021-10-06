using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation
{
    public enum TimePeriods
    {
        [Description("Any time")]
        Any,

        [Description("Last 24 hours")]
        Last24,

        [Description("Last Week")]
        LastWeek,

        [Description("Last 30 days")]
        Last30Days,

        [Description("Last Month")]
        LastMounth,

        [Description("Last Year")]
        LastYear,

        [Description("Custom")]
        Custom
    }
}