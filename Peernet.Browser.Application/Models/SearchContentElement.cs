using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Enums;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.Application.Models
{
    public class SearchContentElement : MvxNotifyPropertyChanged
    {
        private readonly IMvxNavigationService navigationService;
        public MvxObservableCollection<FilterIconModel> FilterIconModels { get; } = new MvxObservableCollection<FilterIconModel>();

        public FilterIconModel FiltersModel { get; }
        public FilterIconModel ColumnsModel { get; }

        public SearchContentElement()
        {
            navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();

            FilterIconModels.Add(new FilterIconModel(FiltersType.All, count: 2357));
            FilterIconModels.Add(new FilterIconModel(FiltersType.Audio, count: 217));
            FilterIconModels.Add(new FilterIconModel(FiltersType.Video, count: 844));
            FilterIconModels.Add(new FilterIconModel(FiltersType.Ebooks, count: 629));
            FilterIconModels.Add(new FilterIconModel(FiltersType.Documents, count: 632));
            FilterIconModels.Add(new FilterIconModel(FiltersType.Pictures, count: 214));
            FilterIconModels.Add(new FilterIconModel(FiltersType.Text, count: 182));
            FilterIconModels.Add(new FilterIconModel(FiltersType.Binary, count: 1));

            ColumnsModel = new FilterIconModel(FiltersType.Columns, true);
            FiltersModel = new FilterIconModel(FiltersType.Filters, true, OpenFilters);
        }

        private void OpenFilters()
        {
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            navigationService.Navigate<FiltersViewModel>();
        }
    }
}