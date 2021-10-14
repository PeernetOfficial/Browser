using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public enum SortOrders
    {
        [Description("Most relevant")]
        MostRelevant,

        [Description("Least relevant")]
        LeastRelevant,

        [Description("Newest")]
        Newest,

        [Description("Oldest")]
        Oldest
    }
}