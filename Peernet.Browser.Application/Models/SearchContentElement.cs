using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Enums;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class SearchContentElement : MvxNotifyPropertyChanged
    {
        private readonly IMvxNavigationService navigationService;

        private readonly Func<SearchFilterResultModel, SearchResultModel> refreshAction;
        public MvxObservableCollection<IconModel> FilterIconModels { get; } = new MvxObservableCollection<IconModel>();

        public MvxObservableCollection<SearchResultRowModel> TableResult { get; } = new MvxObservableCollection<SearchResultRowModel>();

        public IconModel FiltersIconModel { get; }
        public IconModel ColumnsIconModel { get; }

        public FiltersModel Filters { get; }

        private SearchResultModel data;

        public SearchContentElement(Func<SearchFilterResultModel, SearchResultModel> refreshAction)
        {
            if (refreshAction == null) throw new ArgumentNullException();
            this.refreshAction = refreshAction;
            navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();

            Filters = new FiltersModel();
            Filters.Reset(0, 15);
            Filters.Refresh();

            RefreshTable();

            //TODO: statistics came from ??

            FilterIconModels.Add(new IconModel(FiltersType.All, onClick: OnFilterIconClick, count: 2357));
            FilterIconModels.Add(new IconModel(FiltersType.Audio, onClick: OnFilterIconClick, count: 217));
            FilterIconModels.Add(new IconModel(FiltersType.Video, onClick: OnFilterIconClick, count: 844));
            FilterIconModels.Add(new IconModel(FiltersType.Ebooks, onClick: OnFilterIconClick, count: 629));
            FilterIconModels.Add(new IconModel(FiltersType.Documents, onClick: OnFilterIconClick, count: 632));
            FilterIconModels.Add(new IconModel(FiltersType.Pictures, onClick: OnFilterIconClick, count: 214));
            FilterIconModels.Add(new IconModel(FiltersType.Text, onClick: OnFilterIconClick, count: 182));
            FilterIconModels.Add(new IconModel(FiltersType.Binary, onClick: OnFilterIconClick, count: 1));

            ColumnsIconModel = new IconModel(FiltersType.Columns, true);
            FiltersIconModel = new IconModel(FiltersType.Filters, true, OpenFilters);
        }

        private void OnFilterIconClick(IconModel i) => FilterIconModels.Where(x => x != i).Foreach(x => x.IsSelected = false);

        private void RefreshTable()
        {
            data = refreshAction(Filters.SearchFilterResult);
            data.Rows.Foreach(x => x.DownloadAction = Download);
            TableResult.Clear();
            TableResult.AddRange(data.Rows);
        }

        private void Download(SearchResultRowModel row)
        {
        }

        private void OpenFilters(IconModel m)
        {
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            navigationService.Navigate<FiltersViewModel, FiltersModel>(Filters);
        }
    }
}