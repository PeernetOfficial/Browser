using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.ViewModels
{
    public class FiltersViewModel : MvxViewModel
    {
        public FiltersViewModel()
        {
            DateFilters = new CustomFilterModel(GetDateOption());
            FileFormatFilters = new CustomFilterModel(GetFileFormatOption());
            SortOrderFilters = new CustomFilterModel(GetSortOption(), false);
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
            return new[] { "" };
        }

        private string[] GetSortOption()
        {
            return new[] { "" };
        }

        private CustomCheckBoxModel PrepareCheckBox(string text, bool isReset = false)
        {
            return new CustomCheckBoxModel { ResetAll = isReset, Value = text, IsCheckChanged = IsChekcedChanged };
        }

        private void IsChekcedChanged(CustomCheckBoxModel c)
        {
        }
    }
}