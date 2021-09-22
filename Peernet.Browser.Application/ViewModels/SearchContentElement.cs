using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchContentElement : MvxNotifyPropertyChanged
    {
        public MvxObservableCollection<FilterIconModel> FilterIconModels { get; } = new MvxObservableCollection<FilterIconModel>();

        public SearchContentElement()
        {
            FilterIconModels.Add(new FilterIconModel(2357, Filters.All, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(217, Filters.Audio, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(844, Filters.Video, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(629, Filters.Ebooks, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(632, Filters.Documents, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(214, Filters.Pictures, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(182, Filters.Text, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(1, Filters.Binary, FilterSelect));
        }

        private void FilterSelect(FilterIconModel selected)
        {
            selected.IsSelected = !selected.IsSelected;
        }
    }
}