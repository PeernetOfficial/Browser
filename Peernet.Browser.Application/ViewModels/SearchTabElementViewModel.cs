using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Dispatchers;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElementViewModel : ViewModelBase
    {
        private const int increase = 100;
        private readonly Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction;
        private readonly Func<SearchResultRowModel, bool> isPlayerSupported;
        private bool isClearing;
        private int limit = increase;
        private bool showTypeColumn = false;
        private bool showFolderColumn = false;
        private bool showDataFormatColumn = false;
        private bool showDateColumn = true;
        private bool showActionsColumn = true;
        private bool showSharedByColumn = true;
        private bool showSizeColumn = true;

        public SearchTabElementViewModel(
            string title,
            Func<SearchTabElementViewModel, Task> deleteAction,
            Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction,
            Func<SearchResultRowModel, Task> downloadAction,
            Action<SearchResultRowModel> openAction,
            Action<SearchResultRowModel> executePlugAction,
            Func<SearchResultRowModel, bool> isPlayerSupported)
        {
            this.refreshAction = refreshAction;
            this.isPlayerSupported = isPlayerSupported ;

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

            StreamFileCommand = new AsyncCommand<SearchResultRowModel>(model =>
            {
                executePlugAction.Invoke(model);

                return Task.CompletedTask;
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

        public bool ShowTypeColumn
        {
            get => showTypeColumn;
            set
            {
                showTypeColumn = value;
                OnPropertyChanged(nameof(ShowTypeColumn));
            }
        }

        public bool ShowFolderColumn
        {
            get => showFolderColumn;
            set
            {
                showFolderColumn = value;
                OnPropertyChanged(nameof(ShowFolderColumn));
            }
        }

        public bool ShowDataFormatColumn
        {
            get => showDataFormatColumn;
            set
            {
                showDataFormatColumn = value;
                OnPropertyChanged(nameof(ShowDataFormatColumn));
            }
        }

        public bool ShowDateColumn
        {
            get => showDateColumn;
            set
            {
                showDateColumn = value;
                OnPropertyChanged(nameof(ShowDateColumn));
            }
        }

        public bool ShowActionsColumn
        {
            get => showActionsColumn;
            set
            {
                showActionsColumn = value;
                OnPropertyChanged(nameof(ShowActionsColumn));
            }
        }

        public bool ShowSharedByColumn
        {
            get => showSharedByColumn;
            set
            {
                showSharedByColumn = value;
                OnPropertyChanged(nameof(ShowSharedByColumn));
            }
        }

        public bool ShowSizeColumn
        {
            get => showSizeColumn;
            set
            {
                showSizeColumn = value;
                OnPropertyChanged(nameof(ShowSizeColumn));
            }
        }

        public IAsyncCommand<SearchResultRowModel> StreamFileCommand { get; }

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
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Folder", IsChecked = showFolderColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Data Format", IsChecked = showDataFormatColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Type", IsChecked = showTypeColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Date", IsChecked = showDateColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Size", IsChecked = showSizeColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Actions", IsChecked = showActionsColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Shared by", IsChecked = showSharedByColumn, IsCheckChanged = OnColumnCheckboxClick });
            RefreshIconFilters(SearchResultModel.GetDefaultStats().ToDictionary(x => x, y => 0), FilterType.All);
        }

        private void OnColumnCheckboxClick(CustomCheckBoxModel selection)
        {
            switch (selection.Content)
            {
                case "Folder":
                    ShowFolderColumn = selection.IsChecked;
                    break;

                case "Type":
                    ShowTypeColumn = selection.IsChecked;
                    break;

                case "Data Format":
                    ShowDataFormatColumn = selection.IsChecked;
                    break;

                case "Date":
                    ShowDateColumn = selection.IsChecked;
                    break;

                case "Size":
                    ShowSizeColumn = selection.IsChecked;
                    break;

                case "Actions":
                    ShowActionsColumn = selection.IsChecked;
                    break;

                case "Shared by":
                    ShowSharedByColumn = selection.IsChecked;
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
                UIThreadDispatcher.ExecuteOnMainThread(() => TableResult.Clear());
            }
            Filters.SearchFilterResult.LimitOfResult = limit;
            var data = await refreshAction(Filters.SearchFilterResult);
            data.Rows.ForEach(row => row.IsPlayerEnabled = isPlayerSupported.Invoke(row));

            Filters.UuId = data.Id;
            UIThreadDispatcher.ExecuteOnMainThread(() =>
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
            UIThreadDispatcher.ExecuteOnMainThread(() =>
            {
                FilterIconModels.Clear();
                stats.Foreach(x => FilterIconModels.Add(new IconModel(x.Key, onClick: OnFilterIconClick, count: x.Value) { IsSelected = x.Key == selected }));
            });
        }
    }
}