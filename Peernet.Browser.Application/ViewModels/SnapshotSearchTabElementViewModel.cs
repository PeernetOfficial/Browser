using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class SnapshotSearchTabElementViewModel : SearchTabElementViewModel
    {
        public SnapshotSearchTabElementViewModel(
            SearchResultModel searchResult,
            Func<SearchTabElementViewModel, Task> deleteAction,
            ISettingsManager settingsManager,
            IDownloadClient downloadClient,
            Action<DownloadModel> openAction,
            Action<DownloadModel> executePlugAction,
            Func<DownloadModel, bool> isPlayerSupported,
            ISearchService searchService,
            IWarehouseClient warehouseClient,
            IDataTransferManager dataTransferManager,
            IBlockchainService blockchainService)
            : base(deleteAction, settingsManager, downloadClient, openAction, executePlugAction, isPlayerSupported, searchService, warehouseClient, dataTransferManager, blockchainService)
        {
            SearchResult = searchResult;
            Filters = new FiltersModel(new SearchFilterResultModel());
            InitIcons();
            Title = searchResult.Id;
            Loader.Set("Searching...");
            Task.Run(Refresh);
        }

        public override Task Refresh()
        {
            Filters.SearchFilterResult.Limit = PageSize;
            Filters.SearchFilterResult.Offset = (PageIndex - 1) * PageSize;
            return base.Refresh();
        }
    }
}