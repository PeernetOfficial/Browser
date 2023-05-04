using Newtonsoft.Json;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Presentation.Home;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Handlers
{
    public class UriSchemeHandler : IUriSchemeHandler
    {
        private readonly HomeViewModel homeViewModel;
        private readonly IFileClient fileClient;
        private readonly ISettingsManager settingsManager;
        private readonly IDownloadClient downloadClient;
        private readonly ISearchService searchService;
        private readonly IWarehouseClient warehouseClient;
        private readonly IDataTransferManager dataTransferManager;
        private readonly IBlockchainService blockchainService;
        private readonly IUserContext userContext;
        private const string pattern = "peernet:search\\?hash=(?<hash>\\w+)&node=(?<node>\\w+)";

        public UriSchemeHandler(
            HomeViewModel homeViewModel,
            IFileClient fileClient,
            ISettingsManager settingsManager,
            IDownloadClient downloadClient,
            ISearchService searchService,
            IWarehouseClient warehouseClient,
            IDataTransferManager dataTransferManager,
            IBlockchainService blockchainService,
            IUserContext userContext)
        {
            this.homeViewModel = homeViewModel;
            this.settingsManager = settingsManager;
            this.fileClient = fileClient;
            this.downloadClient = downloadClient;
            this.searchService = searchService;
            this.warehouseClient = warehouseClient;
            this.dataTransferManager = dataTransferManager;
            this.blockchainService = blockchainService;
            this.userContext = userContext;
        }

        public async Task Handle(string url)
        {
            var peernetScheme = Parse(url);
            var stream = await fileClient.Read(peernetScheme.Hash, peernetScheme.Node);
            if(stream != null)
            {
                var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();
                var searchResultModel = JsonConvert.DeserializeObject<SearchResultModel>(content);
                var searchTabElementViewModel = new SnapshotSearchTabElementViewModel(
                    searchResultModel,
                    homeViewModel.RemoveTab,
                    settingsManager,
                    downloadClient,
                    homeViewModel.OpenFile,
                    homeViewModel.ExecutePlayButtonPlug,
                    homeViewModel.DoesSupportPlaying,
                    searchService,
                    warehouseClient,
                    dataTransferManager,
                    blockchainService,
                    userContext);
                homeViewModel.AddNewTab(searchTabElementViewModel);
            }
        }

        private PeernetScheme Parse(string url)
        {
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var match = regex.Match(url);
            if (match.Success)
            {
                var groups = match.Groups;
                return new PeernetScheme(groups["hash"].Value, groups["node"].Value);
            }

            return null;
        }
    }

    class PeernetScheme
    {
        public PeernetScheme(string hash, string node)
        {
            Hash = hash;
            Node = node;
        }

        public string Hash { get; set; }

        public string Node { get; set; }
    }
}
