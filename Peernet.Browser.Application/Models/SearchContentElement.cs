using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Enums;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class SearchContentElement : MvxNotifyPropertyChanged
    {
        public MvxObservableCollection<IconModel> FilterIconModels { get; } = new MvxObservableCollection<IconModel>();

        public MvxObservableCollection<SearchResultRowModel> TableResult { get; } = new MvxObservableCollection<SearchResultRowModel>();

        public IconModel FiltersIconModel { get; }
        public IconModel ColumnsIconModel { get; }

        public FiltersModel Filters { get; }

        public IMvxCommand ClearCommand { get; }

        public SearchContentElement(FiltersModel model)
        {
            if (model == null) throw new ArgumentNullException();
            Filters = model;

            ColumnsIconModel = new IconModel(FiltersType.Columns, true);
            FiltersIconModel = new IconModel(FiltersType.Filters, true, OpenFilters);
            ClearCommand = new MvxCommand(() => Filters.Reset(true));

            Refresh();
        }

        private void OnFilterIconClick(IconModel i) => FilterIconModels.Where(x => x != i).Foreach(x => x.IsSelected = false);

        private void Refresh()
        {
            var data = Filters.GetData(Download);
            TableResult.Clear();
            TableResult.AddRange(data.Rows);
            RefreshIconFilters(data.Stats);
        }

        private void RefreshIconFilters(IDictionary<FiltersType, int> stats)
        {
            FilterIconModels.Clear();
            stats.Foreach(x => FilterIconModels.Add(new IconModel(x.Key, onClick: OnFilterIconClick, count: x.Value)));
        }

        private void Download(SearchResultRowModel row)
        {
        }

        private void OpenFilters(IconModel m)
        {
            var navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            Filters.BindFromSearchFilterResult();
            navigationService.Navigate<FiltersViewModel, FiltersModel>(Filters);
        }
    }
}