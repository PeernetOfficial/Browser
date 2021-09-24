using MvvmCross.ViewModels;
using Peernet.Browser.Application.VirtualFileSystem;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchContentElement : MvxNotifyPropertyChanged
    {
        public MvxObservableCollection<FilterIconModel> FilterIconModels { get; } = new MvxObservableCollection<FilterIconModel>();

        public SearchContentElement()
        {
            FilterIconModels.Add(new FilterIconModel(2357, VirtualFileSystemEntityType.All, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(217, VirtualFileSystemEntityType.Audio, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(844, VirtualFileSystemEntityType.Video, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(629, VirtualFileSystemEntityType.Ebook, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(632, VirtualFileSystemEntityType.Document, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(214, VirtualFileSystemEntityType.Picture, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(182, VirtualFileSystemEntityType.Text, FilterSelect));
            FilterIconModels.Add(new FilterIconModel(1, VirtualFileSystemEntityType.Binary, FilterSelect));
        }

        private void FilterSelect(FilterIconModel selected)
        {
            selected.IsSelected = !selected.IsSelected;
        }
    }
}