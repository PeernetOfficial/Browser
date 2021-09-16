using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.ViewModels
{
    public class FiltersViewModel : MvxViewModel
    {
        public FiltersViewModel()
        {
            DateFilters = new CustomFilterModel("Date", GetDateOption());
            FileFormatFilters = new CustomFilterModel("File format", GetFileFormatOption());
            SortOrderFilters = new CustomFilterModel("Sort order", GetSortOption(), false);
        }

        public CustomFilterModel DateFilters { get; }
        public CustomFilterModel FileFormatFilters { get; }
        public CustomFilterModel SortOrderFilters { get; }

        private string[] GetDateOption()
        {
            return new[] { "Any time", "Last 24 hours", "Last Week", "Last 30 Days", "Last Month", "Last Year", "Custom" };
        }

        private string[] GetFileFormatOption()
        {
            return new[] { "All", "PDF file", "Word File", "Website", "Excel File", "Powerpoint File", "EPUB File", "Images", "Movies" };
        }

        private string[] GetSortOption()
        {
            return new[] { "Most relevant", "Least relevean", "Newest", "Oldest" };
        }
    }
}