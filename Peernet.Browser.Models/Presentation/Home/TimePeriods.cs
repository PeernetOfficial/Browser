using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public enum TimePeriods
    {
        [Description("None")]
        None,

        [Description("Last 24 Hours")]
        Last24,

        [Description("Last Week")]
        LastWeek,

        [Description("Last Month")]
        LastMonth,

        [Description("Last Year")]
        LastYear
    }
}