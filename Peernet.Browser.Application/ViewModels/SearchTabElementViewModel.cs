using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElementViewModel : ViewModelBase
    {
        private readonly IUIThreadDispatcher uiThreadDispatcher;

        private const int increase = 100;
        private readonly Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction;
        private bool isClearing;
        private int limit = increase;
        private bool showColumnsDate = true;
        private bool showColumnsDownload = true;
        private bool showColumnsShared = true;
        private bool showColumnsSize = true;

        public SearchTabElementViewModel(IUIThreadDispatcher uiThreadDispatcher, string title, Func<SearchTabElementViewModel, Task> deleteAction, Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction, Func<SearchResultRowModel, Task> downloadAction, Action<SearchResultRowModel> openAction)
        {
            this.uiThreadDispatcher = uiThreadDispatcher;
            this.refreshAction = refreshAction;

            Title = title;

            Filters = new FiltersModel(title);
            Filters.PropertyChanged += async (sender, args) =>
            {
                await Refresh();
            };

            ClearCommand = new AsyncCommand(async () =>
            {
                Filters.Reset(true);
                await Refresh();
            });
            DownloadCommand = new AsyncCommand<SearchResultRowModel>(async (row) => await downloadAction(row));
            DeleteCommand = new AsyncCommand(async () => { await deleteAction(this); });
            OpenCommand = new AsyncCommand<SearchResultRowModel>(
                    model =>
                    {
                        if (openAction != null)
                        {
                            openAction?.Invoke(model);
                        }

                        return Task.CompletedTask;
                    });

            RemoveFilterCommand = new AsyncCommand<SearchFiltersType>(async (type) =>
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

        public IAsyncCommand ClearCommand { get; }

        public ObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new ObservableCollection<CustomCheckBoxModel>();

        public IconModel ColumnsIconModel { get; }

        public IAsyncCommand DeleteCommand { get; }

        public IAsyncCommand<SearchResultRowModel> DownloadCommand { get; }

        public ObservableCollection<IconModel> FilterIconModels { get; } = new ObservableCollection<IconModel>();

        public FiltersModel Filters { get; }

        public IconModel FiltersIconModel { get; }

        public LoadingModel Loader { get; } = new LoadingModel();

        public IAsyncCommand<SearchResultRowModel> OpenCommand { get; }

        public IAsyncCommand<SearchFiltersType> RemoveFilterCommand { get; }

        public bool ShowColumnsDate
        {
            get => showColumnsDate;
            set
            {
                showColumnsDate = value;
                OnPropertyChanged(nameof(ShowColumnsDate));
            }
        }

        public bool ShowColumnsDownload
        {
            get => showColumnsDownload;
            set
            {
                showColumnsDownload = value;
                OnPropertyChanged(nameof(ShowColumnsDownload));
            }
        }

        public bool ShowColumnsShared
        {
            get => showColumnsShared;
            set
            {
                showColumnsShared = value;
                OnPropertyChanged(nameof(ShowColumnsShared));
            }
        }

        public bool ShowColumnsSize
        {
            get => showColumnsSize;
            set
            {
                showColumnsSize = value;
                OnPropertyChanged(nameof(ShowColumnsSize));
            }
        }

        public ObservableCollection<SearchResultRowModel> TableResult { get; } = new ObservableCollection<SearchResultRowModel>();

        public string Title { get; }

        public async Task Initialize()
        {
            await Refresh();
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
            Filters.SearchFilterResult.ShouldReset = true;
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
                uiThreadDispatcher.ExecuteOnMainThread(() => TableResult.Clear());
            }
            Filters.SearchFilterResult.LimitOfResult = limit;
            var data = await refreshAction(Filters.SearchFilterResult);

            Filters.UuId = data.Id;
            uiThreadDispatcher.ExecuteOnMainThread(() =>
            {
                for (var i = TableResult.Count; i < data.Rows.Count; i++)
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