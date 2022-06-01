using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Dispatchers;
using Peernet.SDK.Models.Domain.Search;
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
        private readonly Func<DownloadModel, bool> isPlayerSupported;
        private readonly Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction;
        private ObservableCollection<DownloadModel> activeSearchResults;
        private bool hasMorePages = true;
        private int pageSize = 15;
        private int pageIndex;

        // => pageSize*pageIndex<totalResultsCount
        private int totalResultsCount;

        public SearchTabElementViewModel(
            SearchFilterResultModel searchFilterResultModel,
            Func<SearchTabElementViewModel, Task> deleteAction,
            Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction,
            Func<DownloadModel, Task> downloadAction,
            Action<DownloadModel> openAction,
            Action<DownloadModel> executePlugAction,
            Func<DownloadModel, bool> isPlayerSupported)
        {
            this.refreshAction = refreshAction;
            this.isPlayerSupported = isPlayerSupported;

            Title = searchFilterResultModel.InputText;

            Filters = new FiltersModel(searchFilterResultModel);
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
            Task.Run(async () => await Refresh());
        }

        public ObservableCollection<DownloadModel> ActiveSearchResults
        {
            get => activeSearchResults;
            set
            {
                activeSearchResults = value;
                OnPropertyChanged(nameof(ActiveSearchResults));
            }
        }

        public IAsyncCommand ClearCommand { get; }

        public ObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new ObservableCollection<CustomCheckBoxModel>();

        public IconModel ColumnsIconModel { get; }

        public IAsyncCommand DeleteCommand { get; }

        public IAsyncCommand<DownloadModel> DownloadCommand { get; }

        public ObservableCollection<IconModel> FilterIconModels { get; } = new ObservableCollection<IconModel>();

        public FiltersModel Filters { get; }

        public IconModel FiltersIconModel { get; }

        public bool HasMorePages
        {
            get => hasMorePages;
            set
            {
                hasMorePages = value;
                OnPropertyChanged(nameof(HasMorePages));
            }
        }

        public LoadingModel Loader { get; } = new LoadingModel();

        public IAsyncCommand<DownloadModel> OpenCommand { get; }

        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value;
                OnPropertyChanged(nameof(PageSize));
            }
        }

        public int PageIndex
        {
            get => pageIndex;
            set
            {
                pageIndex = value;
                OnPropertyChanged(nameof(PageIndex));
            }
        }

        public IAsyncCommand<SearchFiltersType> RemoveFilterCommand { get; }

        public IAsyncCommand<DownloadModel> StreamFileCommand { get; }
        public string Title { get; }

        public async Task Initialize()
        {
            await Refresh();
        }

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

        public async Task Refresh()
        {
            Filters.SearchFilterResult.Limit = PageSize;
            Filters.SearchFilterResult.Offset = (PageIndex + 1) * PageSize;

            var data = await refreshAction(Filters.SearchFilterResult);
            data.Rows.ForEach(row => row.IsPlayerEnabled = isPlayerSupported.Invoke(row));

            Filters.UuId = data.Id;
            UIThreadDispatcher.ExecuteOnMainThread(() =>
            {
                ActiveSearchResults = new (data.Rows);
            });

            RefreshIconFilters(data.Stats, data.Filters.FilterType);
            if (ActiveSearchResults.IsNullOrEmpty())
            {
                Loader.Set(GetStatusText(data.Status));
                if(data.Status is SearchStatusEnum.KeepTrying)
                {
                    await Refresh();
                }
            }
            else
            {
                Loader.Reset();
            }
        }

        private void RefreshIconFilters(IDictionary<FilterType, int> stats, FilterType selected)
        {
            UIThreadDispatcher.ExecuteOnMainThread(() =>
            {
                FilterIconModels.Clear();
                stats.Foreach(x => FilterIconModels.Add(new IconModel(x.Key, onClick: OnFilterIconClick, count: x.Value) { IsSelected = x.Key == selected }));
            });
        }

        private string GetStatusText(SearchStatusEnum status)
        {
            switch (status)
            {
                case SearchStatusEnum.IdNotFound:
                    return "Search was terminated.";

                case SearchStatusEnum.KeepTrying:
                    return "Searching...";

                case SearchStatusEnum.NoMoreResults:
                    return "No results.";

                default:
                    return "";
            }
        }
    }
}