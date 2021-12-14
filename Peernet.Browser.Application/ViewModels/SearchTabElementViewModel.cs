using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElementViewModel : MvxViewModel
    {
        private const int increase = 20;
        private readonly Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction;
        private bool isClearing;
        private int limit = increase;
        private bool showColumnsDate = true;
        private bool showColumnsDownload = true;
        private bool showColumnsShared = true;
        private bool showColumnsSize = true;

        public SearchTabElementViewModel(string title, Func<SearchTabElementViewModel, Task> deleteAction, Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction, Func<SearchResultRowModel, Task> downloadAction, Func<SearchResultRowModel, Task> openAction)
        {
            this.refreshAction = refreshAction;
            Title = title;

            Filters = new FiltersModel(title);
            Filters.PropertyChanged += async (sender, args) =>
            {
                await Refresh();
            };

            ClearCommand = new MvxAsyncCommand(async () =>
            {
                Filters.Reset(true);
                await Refresh();
            });
            DownloadCommand = new MvxAsyncCommand<SearchResultRowModel>(async (row) => await downloadAction(row));
            DeleteCommand = new MvxAsyncCommand(async () => { await deleteAction(this); });
            OpenCommand = new MvxAsyncCommand<SearchResultRowModel>(
                    async model =>
                    {
                        if (openAction != null)
                        {
                            await openAction?.Invoke(model);
                        }
                    });

            RemoveFilterCommand = new MvxAsyncCommand<SearchFiltersType>(async (type) =>
            {
                Filters.RemoveAction(type);
                await Refresh();
            });

            ColumnsIconModel = new IconModel(FilterType.Columns, true, OpenCloseColumnsFilter);
            FiltersIconModel = new IconModel(FilterType.Filters, true, OpenCloseFilters);

            InitIcons();
            Loader.Set("Searching...");
            _ = Task.Run(async () => await Refresh(false));
        }

        public IMvxAsyncCommand ClearCommand { get; }

        public MvxObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new MvxObservableCollection<CustomCheckBoxModel>();

        public IconModel ColumnsIconModel { get; }

        public IMvxAsyncCommand DeleteCommand { get; }

        public IMvxAsyncCommand<SearchResultRowModel> DownloadCommand { get; }

        public MvxObservableCollection<IconModel> FilterIconModels { get; } = new MvxObservableCollection<IconModel>();

        public FiltersModel Filters { get; }

        public IconModel FiltersIconModel { get; }

        public LoadingModel Loader { get; } = new LoadingModel();

        public IMvxAsyncCommand<SearchResultRowModel> OpenCommand { get; }

        public IMvxAsyncCommand<SearchFiltersType> RemoveFilterCommand { get; }

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

        public bool ShowColumnsShared
        {
            get => showColumnsShared;
            set => SetProperty(ref showColumnsShared, value);
        }

        public bool ShowColumnsSize
        {
            get => showColumnsSize;
            set => SetProperty(ref showColumnsSize, value);
        }

        public MvxObservableCollection<SearchResultRowModel> TableResult { get; } = new MvxObservableCollection<SearchResultRowModel>();

        public string Title { get; }

        public override async Task Initialize()
        {
            await Refresh();
            await base.Initialize();
        }

        public async Task IsScrollEnd()
        {
            if (isClearing) return;
            var allCount = GetMax();
            if (allCount == 0 || limit > allCount) return;

            limit += increase;
            await Refresh(false);
        }

        public async Task OnSorting(string columnName, DataGridSortingTypeEnum type)
        {
            Filters.SearchFilterResult.SortName = SearchResultRowModel.Parse(columnName);
            Filters.SearchFilterResult.SortType = type;
            await Refresh(true);
        }

        private int GetMax() => (FilterIconModels.FirstOrDefault(x => x.IsSelected)?.Count).GetValueOrDefault();

        private void InitIcons()
        {
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Date", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Size", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Downloads", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Shared by", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            RefreshIconFilters(SearchResultModel.GetDefaultStats().ToDictionary(x => x, y => 0), FilterType.All);
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

                case "Shared by":
                    ShowColumnsShared = selection.IsChecked;
                    break;
            }
        }

        private async Task OnFilterIconClick(IconModel i)
        {
            FilterIconModels.Where(x => x != i).Foreach(x => x.IsSelected = false);
            i.IsSelected = true;
            Filters.SearchFilterResult.FilterType = i.FilterType;
            await Refresh();
        }

        private Task OpenCloseColumnsFilter(IconModel i)
        {
            FiltersIconModel.IsSelected = false;
            return Task.CompletedTask;
        }

        private Task OpenCloseFilters(IconModel m)
        {
            Filters.BindFromSearchFilterResult();
            ColumnsIconModel.IsSelected = false;
            return Task.CompletedTask;
        }

        private async Task Refresh(bool withClear = true)
        {
            if (withClear)
            {
                isClearing = true;
                await GlobalContext.UiThreadDispatcher.ExecuteOnMainThreadAsync(() => TableResult.Clear());
            }
            Filters.SearchFilterResult.LimitOfResult = limit;
            var data = await refreshAction(Filters.SearchFilterResult);

            Filters.UuId = data.Id;
            await GlobalContext.UiThreadDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                for (var i = TableResult.Count; i < data.Rows.Length; i++)
                {
                    TableResult.Add(data.Rows[i]);
                }
            });

            RefreshIconFilters(data.Stats, data.Filters.FilterType);
            if (TableResult.IsNullOrEmpty())
            {
                Loader.Set(data.StatusText);
            }
            else
            {
                Loader.Reset();
            }

            isClearing = false;
        }

        private void RefreshIconFilters(IDictionary<FilterType, int> stats, FilterType selected)
        {
            FilterIconModels.Clear();
            stats.Foreach(x => FilterIconModels.Add(new IconModel(x.Key, onClick: OnFilterIconClick, count: x.Value) { IsSelected = x.Key == selected }));
        }
    }
}