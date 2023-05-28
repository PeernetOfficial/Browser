using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class SnapshotSearchTabElementViewModel : SearchTabElementViewModel
    {
        private readonly Func<DownloadModel, bool> isPlayerSupported;

        public SnapshotSearchTabElementViewModel(
            string title,
            SearchResultModel searchResult,
            Func<SearchTabElementViewModel, Task> deleteAction,
            ISettingsManager settingsManager,
            IDownloadClient downloadClient,
            Action<DownloadModel> openAction,
            Action<DownloadModel> executePlugAction,
            Func<DownloadModel, bool> isPlayerSupported,
            ISearchService searchService,
            IWarehouseClient warehouseClient,
            IFileClient fileClient,
            IDataTransferManager dataTransferManager,
            IBlockchainService blockchainService,
            IUserContext userContext,
            CurrentUserDirectoryViewModel currentUserDirectoryViewModel)
            : base(
                  deleteAction,
                  settingsManager,
                  downloadClient,
                  openAction,
                  executePlugAction,
                  searchService,
                  warehouseClient,
                  fileClient,
                  dataTransferManager,
                  blockchainService,
                  userContext,
                  currentUserDirectoryViewModel)
        {
            this.isPlayerSupported = isPlayerSupported;

            SearchResult = searchResult;
            Filters = new FiltersModel(new SearchFilterResultModel());
            InitIcons();
            Title = title;
            Task.Run(Refresh);
        }

        public override Task Refresh()
        {
            Filters.SearchFilterResult.Limit = PageSize;
            Filters.SearchFilterResult.Offset = (PageIndex - 1) * PageSize;
            Filters.UuId = SearchResult.Id;

            SearchResult.Rows.ForEach(row => row.IsPlayerEnabled = isPlayerSupported.Invoke(row));
            ActiveSearchResults = new(SearchResult.Rows.Take(new Range(PageSize * (PageIndex - 1), PageSize * PageIndex)));
            PagesCount = (int)Math.Ceiling(SearchResult.Rows.Count / (double)PageSize);

            RefreshIconFilters(SearchResult.Stats, SearchResult.Filters.FilterType);

            return Task.CompletedTask;
        }
    }
}