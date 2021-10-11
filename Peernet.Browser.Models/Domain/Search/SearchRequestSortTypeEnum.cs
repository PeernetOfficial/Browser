using System.ComponentModel;

namespace Peernet.Browser.Models.Domain.Search
{
    public enum SearchRequestSortTypeEnum
    {
        [Description("No sorting. Results are returned as they come in.")]
        SortNone = 0,

        [Description("Least relevant results first.")]
        SortRelevanceAsc,

        [Description("Most relevant results first.")]
        SortRelevanceDec,

        [Description("Oldest first.")]
        SortDateAsc,

        [Description("Newest first.")]
        SortDateDesc,

        [Description("File name ascending. The folder name is not used for sorting.")]
        SortNameAsc,

        [Description("File name descending. The folder name is not used for sorting.")]
        SortNameDesc
    }
}