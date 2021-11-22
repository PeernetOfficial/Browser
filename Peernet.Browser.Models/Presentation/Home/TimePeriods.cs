using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public enum TimePeriods
    {
        [Description("Last 24 hours")]
        Last24,

        [Description("Last Week")]
        LastWeek,

        [Description("Last Month")]
        LastMonth,

        [Description("Last Year")]
        LastYear,

        [Description("Custom")]
        Custom
    }
}