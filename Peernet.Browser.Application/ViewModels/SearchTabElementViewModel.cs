using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Dispatchers;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation.Footer;
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
        private readonly Func<DownloadModel, bool> isPlayerSupported;
        private bool isClearing;
        private int limit = increase;

        public SearchTabElementViewModel(
            string title,
            Func<SearchTabElementViewModel, Task> deleteAction,
            Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction,
            Func<DownloadModel, Task> downloadAction,
            Action<DownloadModel> openAction,
            Action<DownloadModel> executePlugAction,
            Func<DownloadModel, bool> isPlayerSupported)
        {
            this.refreshAction = refreshAction;
            this.isPlayerSupported = isPlayerSupported;

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
            DownloadCommand = new AsyncCommand<DownloadModel>(async (row) => await downloadAction(row));
            DeleteCommand = new AsyncCommand(async () => { await deleteAction(this); });
            OpenCommand = new AsyncCommand<DownloadModel>(
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

            StreamFileCommand = new AsyncCommand<DownloadModel>(model =>
            {
                executePlugAction.Invoke(model);

                return Task.CompletedTask;
            });

            ColumnsIconModel = new IconModel(FilterType.Columns, true);
            FiltersIconModel = new IconModel(FilterType.Filters, true, OpenCloseFilters);

            InitIcons();
            Loader.Set("Searching...");
            _ = Task.Run(async () => await Refresh(false));
        }

        public IAsyncCommand ClearCommand { get; }

        public ObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new ObservableCollection<CustomCheckBoxModel>();

        public IconModel ColumnsIconModel { get; }

        public IAsyncCommand DeleteCommand { get; }

        public IAsyncCommand<DownloadModel> DownloadCommand { get; }

        public ObservableCollection<IconModel> FilterIconModels { get; } = new ObservableCollection<IconModel>();

        public FiltersModel Filters { get; }

        public IconModel FiltersIconModel { get; }

        public LoadingModel Loader { get; } = new LoadingModel();

        public IAsyncCommand<DownloadModel> OpenCommand { get; }

        public IAsyncCommand<SearchFiltersType> RemoveFilterCommand { get; }

        public IAsyncCommand<DownloadModel> StreamFileCommand { get; }

        public ObservableCollection<DownloadModel> TableResult { get; } = new();

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

        private int GetMax() => (FilterIconModels.FirstOrDefault(x => x.IsSelected)?.Count).GetValueOrDefault();

        private void InitIcons()
        {
            RefreshIconFilters(SearchResultModel.GetDefaultStats().ToDictionary(x => x, y => 0), FilterType.All);
        }

        private async Task OnFilterIconClick(IconModel i)
        {
            FilterIconModels.Where(x => x != i).Foreach(x => x.IsSelected = false);
            i.IsSelected = true;
            Filters.SearchFilterResult.FilterType = i.FilterType;
            Filters.SearchFilterResult.ShouldReset = true;
            await Refresh();
        }

        private Task OpenCloseFilters(IconModel m)
        {
            Filters.BindFromSearchFilterResult();
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