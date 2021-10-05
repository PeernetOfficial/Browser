using System.ComponentModel;

namespace Peernet.Browser.Application.Enums
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