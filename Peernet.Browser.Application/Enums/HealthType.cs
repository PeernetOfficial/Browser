using System.ComponentModel;

namespace Peernet.Browser.Application.Enums
{
    public enum HealthType
    {
        [Description("Grean health indicator")]
        Grean,

        [Description("Yellow health indicator")]
        Yellow,

        [Description("Red health indicator")]
        Red
    }
}