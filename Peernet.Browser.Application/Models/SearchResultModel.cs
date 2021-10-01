namespace Peernet.Browser.Application.Models
{
    public class SearchResultModel
    {
        public int Id { get; set; }
        public SearchResultRowModel[] Rows { get; set; } = new SearchResultRowModel[0];

        public SearchFilterResultModel Filters { get; set; }
    }
}