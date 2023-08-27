using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Common;
using Peernet.Browser.Application.Widgets;
using Peernet.SDK.Models.Domain.Search;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public abstract class SearchTabElementViewModel : ViewModelBase
    {
        private readonly IBlockchainService blockchainService;
        private readonly IDataTransferManager dataTransferManager;
        private readonly Func<SearchTabElementViewModel, Task> deleteAction;
        private readonly IDownloadClient downloadClient;
        private readonly Action<DownloadModel> executePlugAction;
        private readonly Action<DownloadModel> openAction;
        private readonly ISearchService searchService;
        private readonly ISettingsManager settingsManager;
        private readonly IWarehouseClient warehouseClient;
        private readonly IFileClient fileClient;
        private readonly IUserContext userContext;
        private ObservableCollection<DownloadModel> activeSearchResults;
        private readonly CurrentUserDirectoryViewModel currentUserDirectoryViewModel;
        private int pageIndex = 1;
        private int pagesCount;
        private int pageSize = 15;
        private ViewType viewType;
        private DownloadModel selectedItem;

        public SearchTabElementViewModel(
            IWidgetsService widgetsService,
            Func<SearchTabElementViewModel, Task> deleteAction,
            ISettingsManager settingsManager,
            IDownloadClient downloadClient,
            Action<DownloadModel> openAction,
            Action<DownloadModel> executePlugAction,
            ISearchService searchService,
            IWarehouseClient warehouseClient,
            IFileClient fileClient,
            IDataTransferManager dataTransferManager,
            IBlockchainService blockchainService,
            IUserContext userContext,
            CurrentUserDirectoryViewModel currentUserDirectoryViewModel)
        {
            this.settingsManager = settingsManager;
            this.downloadClient = downloadClient;
            this.warehouseClient = warehouseClient;
            this.fileClient = fileClient;
            this.dataTransferManager = dataTransferManager;
            this.blockchainService = blockchainService;
            this.searchService = searchService;
            this.deleteAction = deleteAction;
            this.openAction = openAction;
            this.executePlugAction = executePlugAction;
            this.userContext = userContext;
            WidgetsService = widgetsService;

            ColumnsIconModel = new IconModel(FilterType.Columns, true);
            FiltersIconModel = new IconModel(FilterType.Filters, true, OpenCloseFilters);

            InitIcons();
            this.currentUserDirectoryViewModel = currentUserDirectoryViewModel;
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
            new AsyncCommand<ViewType>(viewType =>
            {
                ViewType = viewType;
                
                return Task.CompletedTask;
            });

        public IAsyncCommand ClearCommand => new AsyncCommand(async () =>
        {
            Filters.Reset(true);
            await Refresh();
        });

        public ObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new ObservableCollection<CustomCheckBoxModel>();
        public IconModel ColumnsIconModel { get; set; }
        public IAsyncCommand DeleteCommand => new AsyncCommand(async () => { await deleteAction(this); });
        public IAsyncCommand<DownloadModel> DownloadCommand => new AsyncCommand<DownloadModel>(DownloadFile);

        public IAsyncCommand<DownloadModel> JoinChatCommand => new AsyncCommand<DownloadModel>(JoinChat);

        private Task JoinChat(DownloadModel model)
        {
            var chatMessage = new ChatMessage
            {
                Author = userContext.User,
                Content = "This is dummy message for test purposes!"
            };
            var chatMessage2 = new ChatMessage
            {
                Author = userContext.User,
                Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
            };
            var chatMessage3 = new ChatMessage
            {
                Author = userContext.User,
                Content = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature."
            };
            var chatMessage4 = new ChatMessage
            {
                Author = userContext.User,
                Content = "The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham."
            };

            var chatMessage5 = new ChatMessage
            {
                Author = userContext.User,
                Content = "The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham."
            };

            var chatMessage6 = new ChatMessage
            {
                Author = userContext.User,
                Content = "The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham."
            };

            model.Chat = new ChatModel(new List<ChatMessage> { chatMessage, chatMessage2, chatMessage3, chatMessage4, chatMessage5, chatMessage6 });
        
            return Task.CompletedTask;
        }

        public ObservableCollection<IconModel> FilterIconModels { get; } = new ObservableCollection<IconModel>();
        public FiltersModel Filters { get; protected set; }
        public IconModel FiltersIconModel { get; protected set; }
        public IAsyncCommand FirstPageCommand => new AsyncCommand(GoToFirstPage);
        public IAsyncCommand LastPageCommand => new AsyncCommand(GoToLastPage);

        public LoadingModel Loader { get; } = new LoadingModel();

        public IAsyncCommand NextPageCommand => new AsyncCommand(GoToNextPage);

        public IAsyncCommand<DownloadModel> OpenCommand => new AsyncCommand<DownloadModel>(
                    model =>
                    {
                        if (openAction != null)
                        {
                            openAction?.Invoke(model);
                        }

                        return Task.CompletedTask;
                    });

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


        public IAsyncCommand<SearchFiltersType> RemoveFilterCommand => new AsyncCommand<SearchFiltersType>(async (type) =>
        {
            Filters.RemoveAction(type);
            await Refresh();
        });

        public IAsyncCommand<DownloadModel> StreamFileCommand => new AsyncCommand<DownloadModel>(model =>
        {
            executePlugAction.Invoke(model);

            return Task.CompletedTask;
        });

        public string Title { get; protected set; }
        protected SearchResultModel SearchResult { get; set; }

        public async Task<FileModel> CreateResultsSnapshot()
        {
            var snapshot = new ResultsSnapshot
            {
                Title = Title,
                SearchResultModel = SearchResult,
                PeernetSchemaViewType = PeernetSchemaViewType.Search
            };

            var path = await searchService.CreateSnapshot(snapshot);
            var fileModel = new FileModel(path);
            var upload = new Upload(warehouseClient, fileModel);
            await dataTransferManager.QueueUp(upload);

            if (upload.File.Hash != null)
            {
                var format = await fileClient.GetFormat(path);
                upload.File.Format = format.FileFormat;
                upload.File.Type = format.FileType;
                await blockchainService.AddFiles(new[] { fileModel });
                upload.File.NodeId = Enumerable.Range(0, userContext.NodeId.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(userContext.NodeId.Substring(x, 2), 16))
                     .ToArray();
                await currentUserDirectoryViewModel.ReloadVirtualFileSystem();
            }

            return fileModel;
        }

        public abstract Task Refresh();

        protected void InitIcons()
        {
            RefreshIconFilters(SearchResultModel.GetDefaultStats().ToDictionary(x => x, y => 0), FilterType.All);
        }

        protected Task OpenCloseFilters(IconModel m)
        {
            Filters.BindFromSearchFilterResult();
            return Task.CompletedTask;
        }

        protected void RefreshIconFilters(IDictionary<FilterType, int> stats, FilterType selected)
        {
            UIThreadDispatcher.ExecuteOnMainThread(() =>
            {
                FilterIconModels.Clear();
                stats.Foreach(x => FilterIconModels.Add(new IconModel(x.Key, onClick: OnFilterIconClick, count: x.Value) { IsSelected = x.Key == selected }));
            });
        }

        private async Task DownloadFile(DownloadModel model)
        {
            var filePath = Path.Combine(settingsManager.DownloadPath, UtilityHelper.StripInvalidWindowsCharactersFromFileName(model.File.Name));
            await dataTransferManager.QueueUp(new SDK.Models.Presentation.Download(downloadClient, model.File, filePath));
        }

        protected string GetStatusText(SearchStatusEnum status)
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

        private async Task OnFilterIconClick(IconModel i)
        {
            FilterIconModels.Where(x => x != i).Foreach(x => x.IsSelected = false);
            i.IsSelected = true;
            Filters.SearchFilterResult.FilterType = i.FilterType;
            Filters.SearchFilterResult.ShouldReset = true;
            await Refresh();
        }
    }
}