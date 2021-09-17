using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.ViewModels
{
    public class FiltersViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;

        public FiltersViewModel(IMvxNavigationService mvxNavigationService)
        {
            this.mvxNavigationService = mvxNavigationService;

            DateFilters = new CustomFilterModel("Date", GetDateOption());
            FileFormatFilters = new CustomFilterModel("File format", GetFileFormatOption());
            SortOrderFilters = new CustomFilterModel("Sort order", GetSortOption(), false);
            RangeFilter = new RangeSliderModel { Min = 10, Max = 90, CurrentMin = 20, CurrentMax = 80 };

            CancelCommand = new MvxCommand(Hide);
            ApplyFiltersCommand = new MvxCommand(ApplyFilters);
        }

        public CustomFilterModel DateFilters { get; }
        public CustomFilterModel FileFormatFilters { get; }
        public CustomFilterModel SortOrderFilters { get; }
        public RangeSliderModel RangeFilter { get; }

        public IMvxCommand CancelCommand { get; }

        public IMvxCommand ApplyFiltersCommand { get; }

        private void Hide()
        {
            GlobalContext.IsMainWindowActive = true;
            mvxNavigationService.Close(this);
        }

        private void ApplyFilters()
        {
            //TODO: pass filter models
            Hide();
        }

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