using System.Collections.Generic;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class SearchResultModel
    {
        public string Id { get; set; }
        public SearchResultRowModel[] Rows { get; set; } = new SearchResultRowModel[0];

        public SearchFilterResultModel Filters { get; set; }

        public IDictionary<FiltersType, int> Stats { get; set; }

        public static FiltersType[] GetDefaultStats()
        {
            return new[]
            {
                FiltersType.All,
                FiltersType.Audio,
                FiltersType.Video,
                FiltersType.Ebooks,
                FiltersType.Documents,
                FiltersType.Pictures,
                FiltersType.Text,
                FiltersType.Binary
            };
        }
    }
}