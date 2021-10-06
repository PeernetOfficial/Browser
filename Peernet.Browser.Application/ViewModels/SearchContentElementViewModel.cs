using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Models.Extensions;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchContentElementViewModel : MvxNotifyPropertyChanged
    {
        public SearchContentElementViewModel(FiltersModel model)
        {
            Filters = model ?? throw new ArgumentNullException();
            Filters.CloseAction += (x) => { if (x) Refresh(); };

            ColumnsIconModel = new IconModel(FiltersType.Columns, true);
            FiltersIconModel = new IconModel(FiltersType.Filters, true, OpenFilters);
            ClearCommand = new MvxCommand(() => Filters.Reset(true));

            Refresh();
        }

        public IMvxCommand ClearCommand { get; }
        public IconModel ColumnsIconModel { get; }
        public MvxObservableCollection<IconModel> FilterIconModels { get; } = new MvxObservableCollection<IconModel>();

        public FiltersModel Filters { get; }
        public IconModel FiltersIconModel { get; }
        public MvxObservableCollection<SearchResultRowModel> TableResult { get; } = new MvxObservableCollection<SearchResultRowModel>();

        private void Download(SearchResultRowModel row)
        {
        }

        private void OnFilterIconClick(IconModel i)
        {
            FilterIconModels.Where(x => x != i).Foreach(x => x.IsSelected = false);
            i.IsSelected = true;
            Filters.SearchFilterResult.FilterType = i.FilterType;
            Refresh();
        }

        private void OpenFilters(IconModel m)
        {
            var navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            Filters.BindFromSearchFilterResult();
            navigationService.Navigate<FiltersViewModel, FiltersModel>(Filters);
        }

        // todo: it is executed synchronously in constructor -> it should be fixed. As suggested earlier, if that was real viewmodel it could override asynchronous OnInitialize
        private void Refresh()
        {
            var data = Task.Run(async () => await Filters.GetData(Download))
                .GetResultBlockingWithoutContextSynchronization();
            TableResult.Clear();
            data.Rows.Foreach(x => TableResult.Add(x));
            RefreshIconFilters(data.Stats, data.Filters.FilterType);
        }

        private void RefreshIconFilters(IDictionary<FiltersType, int> stats, FiltersType selected)
        {
            FilterIconModels.Clear();
            stats.Foreach(x => FilterIconModels.Add(new IconModel(x.Key, onClick: OnFilterIconClick, count: x.Value) { IsSelected = x.Key == selected }));
        }
    }
}