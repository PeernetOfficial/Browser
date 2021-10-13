using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchContentElementViewModel : MvxNotifyPropertyChanged
    {
        public SearchContentElementViewModel(FiltersModel model)
        {
            Filters = model ?? throw new ArgumentNullException();
            Filters.CloseAction += (x) => { if (x) Refresh(); };

            ColumnsIconModel = new IconModel(FiltersType.Columns, true, ShowColumnSelection);
            FiltersIconModel = new IconModel(FiltersType.Filters, true, OpenFilters);
            ClearCommand = new MvxCommand(() => Filters.Reset(true));
            InitIcons();
            Refresh();
        }

        public void OnSorting(string columnName, DataGridSortingTypeEnum type)
        {
            Filters.SearchFilterResult.SortName = SearchResultRowModel.Parse(columnName);
            Filters.SearchFilterResult.SortType = type;
            Refresh();
        }

        private bool showColumnsSelector;

        public bool ShowColumnsSelector
        {
            get => showColumnsSelector;
            set => SetProperty(ref showColumnsSelector, value);
        }

        private bool showColumnsSize = true;
        private bool showColumnsDate = true;
        private bool showColumnsDownload = true;

        public bool ShowColumnsSize
        {
            get => showColumnsSize;
            set => SetProperty(ref showColumnsSize, value);
        }

        public bool ShowColumnsDate
        {
            get => showColumnsDate;
            set => SetProperty(ref showColumnsDate, value);
        }

        public bool ShowColumnsDownload
        {
            get => showColumnsDownload;
            set => SetProperty(ref showColumnsDownload, value);
        }

        public MvxObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new MvxObservableCollection<CustomCheckBoxModel>();

        public IMvxCommand ClearCommand { get; }
        public IconModel ColumnsIconModel { get; }
        public MvxObservableCollection<IconModel> FilterIconModels { get; } = new MvxObservableCollection<IconModel>();

        public FiltersModel Filters { get; }
        public IconModel FiltersIconModel { get; }
        public MvxObservableCollection<SearchResultRowModel> TableResult { get; } = new MvxObservableCollection<SearchResultRowModel>();

        private void Download(SearchResultRowModel row)
        {
        }

        private void InitIcons()
        {
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Date", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Size", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Downloads", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            RefreshIconFilters(SearchResultModel.GetDefaultStats().ToDictionary(x => x, y => 0), FiltersType.All);
        }

        private void OnColumnCheckboxClick(CustomCheckBoxModel selection)
        {
            switch (selection.Content)
            {
                case "Date":
                    ShowColumnsDate = selection.IsChecked;
                    break;

                case "Size":
                    ShowColumnsSize = selection.IsChecked;
                    break;

                case "Downloads":
                    ShowColumnsDownload = selection.IsChecked;
                    break;
            }
        }

        private void OnFilterIconClick(IconModel i)
        {
            FilterIconModels.Where(x => x != i).Foreach(x => x.IsSelected = false);
            i.IsSelected = true;
            Filters.SearchFilterResult.FilterType = i.FilterType;
            Refresh();
        }

        private void ShowColumnSelection(IconModel i)
        {
            ShowColumnsSelector = !ShowColumnsSelector;
        }

        private void OpenFilters(IconModel m)
        {
            var navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            Filters.BindFromSearchFilterResult();
            navigationService.Navigate<FiltersViewModel, FiltersModel>(Filters);
        }

        private async void Refresh()
        {
            var data = await Filters.GetData(Download);
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