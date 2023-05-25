using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Domain.Search;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class ActiveSearchTabElementViewModel : SearchTabElementViewModel
    {
        private readonly ISearchService searchService;
        private readonly IUserContext userContext;
        private readonly Func<DownloadModel, bool> isPlayerSupported;

        public ActiveSearchTabElementViewModel(
            SearchFilterResultModel searchFilterResultModel,
            Func<SearchTabElementViewModel, Task> deleteAction,
            ISettingsManager settingsManager,
            IDownloadClient downloadClient,
            Action<DownloadModel> openAction,
            Action<DownloadModel> executePlugAction,
            Func<DownloadModel, bool> isPlayerSupported,
            ISearchService searchService,
            IWarehouseClient warehouseClient,
            IDataTransferManager dataTransferManager,
            IBlockchainService blockchainService,
            IUserContext userContext,
            CurrentUserDirectoryViewModel currentUserDirectoryViewModel)
            : base(deleteAction, settingsManager, downloadClient, openAction, executePlugAction, searchService, warehouseClient, dataTransferManager, blockchainService, userContext, currentUserDirectoryViewModel)
        {
            this.searchService = searchService;
            this.userContext = userContext;
            this.isPlayerSupported = isPlayerSupported;

            Title = searchFilterResultModel.InputText;

            Filters = new FiltersModel(searchFilterResultModel);
            Filters.PropertyChanged += async (sender, args) =>
            {
                await Refresh();
            };

            Loader.Set("Searching...");
            Task.Run(Refresh);
        }

        public override async Task Refresh()
        {
            Filters.SearchFilterResult.Limit = PageSize;
            Filters.SearchFilterResult.Offset = (PageIndex - 1) * PageSize;
            SearchResult = await searchService.Search(Filters.SearchFilterResult);
            
            SearchResult.Rows.ForEach(row => row.IsPlayerEnabled = isPlayerSupported.Invoke(row));

            Filters.UuId = SearchResult.Id;

            ActiveSearchResults = new(SearchResult.Rows);
            PagesCount = (int)Math.Ceiling(SearchResult.Stats[Filters.SearchFilterResult.FilterType] / (double)PageSize);

            RefreshIconFilters(SearchResult.Stats, SearchResult.Filters.FilterType);
            if (ActiveSearchResults.IsNullOrEmpty())
            {
                Loader.Set(GetStatusText(SearchResult.Status));
                if (SearchResult.Status is SearchStatusEnum.KeepTrying)
                {
                    await Refresh();
                }
            }
            else
            {
                Loader.Reset();
            }
        }

        public IAsyncCommand FilterOwnFiles => new AsyncCommand(async () =>
        {
            Filters.SearchFilterResult.NodeId = userContext.NodeId;
            await Refresh();
        });
    }
}