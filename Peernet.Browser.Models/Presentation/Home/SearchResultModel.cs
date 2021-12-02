using System.Collections.Generic;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class SearchResultModel
    {
        public string Id { get; set; }
        public string StatusText { get; set; }
        public SearchResultRowModel[] Rows { get; set; } = new SearchResultRowModel[0];

        public SearchFilterResultModel Filters { get; set; }

        public IDictionary<FilterType, int> Stats { get; set; }

        public static FilterType[] GetDefaultStats()
        {
            return new[]
            {
                FilterType.All,
                FilterType.Audio,
                FilterType.Video,
                FilterType.Ebooks,
                FilterType.Documents,
                FilterType.Pictures,
                FilterType.Text,
                FilterType.Binary
            };
        }
    }
}