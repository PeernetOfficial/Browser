using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.ViewModels
{
    public class FiltersViewModel : MvxViewModel
    {
        public MvxObservableCollection<CustomCheckBoxModel> DateFilters { get; } = new MvxObservableCollection<CustomCheckBoxModel>();
        public MvxObservableCollection<CustomCheckBoxModel> FileFormatFilters { get; } = new MvxObservableCollection<CustomCheckBoxModel>();
        public MvxObservableCollection<CustomCheckBoxModel> SortOrderFilters { get; } = new MvxObservableCollection<CustomCheckBoxModel>();

        private void Init()
        {
            DateFilters.Add(PrepareCheckBox("Any time", true));
            DateFilters.Add(PrepareCheckBox("Last 24 hours"));
            DateFilters.Add(PrepareCheckBox("Last Week"));
            DateFilters.Add(PrepareCheckBox("Last 30 Days"));
            DateFilters.Add(PrepareCheckBox("Last Month"));
            DateFilters.Add(PrepareCheckBox("Last Year"));
            DateFilters.Add(PrepareCheckBox("Custom"));
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