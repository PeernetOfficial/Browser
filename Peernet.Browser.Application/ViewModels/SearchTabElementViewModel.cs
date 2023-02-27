using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Widgets;
using Peernet.SDK.Models.Domain.Search;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation;
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
        private int pageIndex = 1;
        private int pagesCount;
        private int pageSize = 15;
        private ViewType viewType;
        private DownloadModel selectedItem;

        public SearchTabElementViewModel(
            IWidgetsService widgetsService,
            SearchFilterResultModel searchFilterResultModel,
            Func<SearchTabElementViewModel, Task> deleteAction,
            Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction,
            Func<DownloadModel, Task> downloadAction,
            Action<DownloadModel> openAction,
            Action<DownloadModel> executePlugAction,
            Func<DownloadModel, bool> isPlayerSupported)
        {
            WidgetsService = widgetsService;
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

        public IAsyncCommand<ViewType> ChangeViewCommand =>
            new AsyncCommand<ViewType>(
                viewType =>
                {
                    ViewType = viewType;

                    return Task.CompletedTask;
                });

        public IAsyncCommand ClearCommand { get; }

        public ObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new ObservableCollection<CustomCheckBoxModel>();

        public IconModel ColumnsIconModel { get; }

        public IAsyncCommand DeleteCommand { get; }

        public IAsyncCommand<DownloadModel> DownloadCommand { get; }

        public ObservableCollection<IconModel> FilterIconModels { get; } = new ObservableCollection<IconModel>();

        public FiltersModel Filters { get; }

        public IconModel FiltersIconModel { get; }

        public IAsyncCommand FirstPageCommand => new AsyncCommand(GoToFirstPage);

        public IAsyncCommand LastPageCommand => new AsyncCommand(GoToLastPage);

        public LoadingModel Loader { get; } = new LoadingModel();

        public IAsyncCommand NextPageCommand => new AsyncCommand(GoToNextPage);

        public IAsyncCommand<DownloadModel> OpenCommand { get; }

        public DownloadModel SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public int PageIndex
        {
            get => pageIndex;
            set
            {
                if (value > PagesCount)
                {
                    pageIndex = PagesCount;
                }
                else if (value <= 0)
                {
                    pageIndex = 1;
                }
                else
                {
                    pageIndex = value;
                }
                OnPropertyChanged(nameof(PageIndex));
            }
        }

        public int PagesCount
        {
            get => pagesCount;
            set
            {
                pagesCount = value;
                OnPropertyChanged(nameof(PagesCount));
            }
        }

        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value;

                // Could be GoToPage(1) but I relay on PropertyChanged Handlers from code behind as they execute concurrently
                PageIndex = 1;
                OnPropertyChanged(nameof(PageSize));
            }
        }

        public IAsyncCommand PreviousPageCommand => new AsyncCommand(GoToPreviousPage);

        public IAsyncCommand<SearchFiltersType> RemoveFilterCommand { get; }

        public IAsyncCommand<DownloadModel> StreamFileCommand { get; }

        public string Title { get; }

        public ViewType ViewType
        {
            get => viewType;
            set
            {
                viewType = value;
                OnPropertyChanged(nameof(ViewType));
            }
        }

        public IWidgetsService WidgetsService { get; }

        public async Task Initialize()
        {
            await Refresh();
        }

        public async Task Refresh()
        {
            Filters.SearchFilterResult.Limit = PageSize;
            Filters.SearchFilterResult.Offset = (PageIndex - 1) * PageSize;

            var searchResultModel = await refreshAction(Filters.SearchFilterResult);
            searchResultModel.Rows.ForEach(row => row.IsPlayerEnabled = isPlayerSupported.Invoke(row));

            Filters.UuId = searchResultModel.Id;
            UIThreadDispatcher.ExecuteOnMainThread(() =>
            {
                ActiveSearchResults = new(searchResultModel.Rows);
                PagesCount = (int)Math.Ceiling(searchResultModel.Stats[Filters.SearchFilterResult.FilterType] / (double)PageSize);
            });

            RefreshIconFilters(searchResultModel.Stats, searchResultModel.Filters.FilterType);
            if (ActiveSearchResults.IsNullOrEmpty())
            {
                Loader.Set(GetStatusText(searchResultModel.Status));
                if (searchResultModel.Status is SearchStatusEnum.KeepTrying)
                {
                    await Refresh();
                }
            }
            else
            {
                Loader.Reset();
            }
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

        private async Task GoToFirstPage() => await GoToPage(1);

        private async Task GoToLastPage() => await GoToPage(PagesCount);

        private async Task GoToNextPage()
        {
            if (PageIndex < PagesCount)
            {
                await GoToPage(PageIndex + 1);
            }
        }

        private async Task GoToPage(int pageIndex)
        {
            PageIndex = pageIndex;
            await Refresh();
        }

        private async Task GoToPreviousPage()
        {
            if (PageIndex > 1)
            {
                await GoToPage(PageIndex - 1);
            }
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