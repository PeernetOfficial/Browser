using Newtonsoft.Json;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Domain.Search;
using Peernet.SDK.Models.Presentation;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Handlers
{
    public class UriSchemeHandler : IUriSchemeHandler
    {
        private const string pattern = "peernet:\\/\\/hash=(?<hash>\\w+)&node=(?<node>\\w+)";
        private readonly IBlockchainService blockchainService;
        private readonly IDataTransferManager dataTransferManager;
        private readonly IDownloadClient downloadClient;
        private readonly IFileClient fileClient;
        private readonly MainViewModel mainViewModel;
        private readonly HomeViewModel homeViewModel;
        private readonly DirectoryViewModel directoryViewModel;
        private readonly ISearchService searchService;
        private readonly ISettingsManager settingsManager;
        private readonly IUserContext userContext;
        private readonly IWarehouseClient warehouseClient;

        public UriSchemeHandler(
            MainViewModel mainViewModel,
            HomeViewModel homeViewModel,
            DirectoryViewModel directoryViewModel,
            IFileClient fileClient,
            ISettingsManager settingsManager,
            IDownloadClient downloadClient,
            ISearchService searchService,
            IWarehouseClient warehouseClient,
            IDataTransferManager dataTransferManager,
            IBlockchainService blockchainService,
            IUserContext userContext)
        {
            this.mainViewModel = mainViewModel;
            this.homeViewModel = homeViewModel;
            this.directoryViewModel = directoryViewModel;
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
            if (stream != null)
            {
                var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();
                var resultsSnapshot = JsonConvert.DeserializeObject<ResultsSnapshot>(content);

                switch (resultsSnapshot.PeernetSchemaViewType)
                {
                    case PeernetSchemaViewType.Search:
                        var searchTabElementViewModel = new SnapshotSearchTabElementViewModel(
                            resultsSnapshot.Title,
                            resultsSnapshot.SearchResultModel,
                            homeViewModel.RemoveTab,
                            settingsManager,
                            downloadClient,
                            homeViewModel.OpenFile,
                            homeViewModel.ExecutePlayButtonPlug,
                            homeViewModel.DoesSupportPlaying,
                            searchService,
                            warehouseClient,
                            fileClient,
                            dataTransferManager,
                            blockchainService,
                            userContext,
                            directoryViewModel.CurrentUserDirectoryViewModel);
                        homeViewModel.AddNewTab(searchTabElementViewModel);
                        break;

                    case PeernetSchemaViewType.Directory:
                        var searchResult = JsonConvert.DeserializeObject<SearchResult>(resultsSnapshot.SearchResultModel.Snapshot);
                        await mainViewModel.DirectoryViewModel.AddTab(resultsSnapshot.Title, searchResult);
                        mainViewModel.SelectedIndex = mainViewModel.DirectoryViewModel.GetNavigationIndex();
                        break;
                }
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

    internal class PeernetScheme
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