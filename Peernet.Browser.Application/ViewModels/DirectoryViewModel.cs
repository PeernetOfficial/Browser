using DevExpress.Mvvm.Native;
using Newtonsoft.Json;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Search;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Plugins;
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
    public class DirectoryViewModel : NavigationItemViewModelBase
    {
        private readonly IFileClient fileClient;
        private readonly IBlockchainService blockchainService;
        private readonly IDataTransferManager dataTransferManager;
        private readonly IMergeClient mergeClient;
        private readonly IEnumerable<IPlayButtonPlug> playButtonPlugs;
        private readonly ISearchService searchService;
        private readonly IUserContext userContext;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private readonly IWarehouseClient warehouseClient;
        private readonly IProfileService profileService;
        private int selectedIndex = 0;

        public DirectoryViewModel(
            IUserContext userContext,
            IMergeClient mergeClient,
            IBlockchainService blockchainService,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IEnumerable<IPlayButtonPlug> playButtonPlugs,
            ISearchService searchService,
            IWarehouseClient warehouseClient,
            IFileClient fileClient,
            IDataTransferManager dataTransferManager,
            IProfileService profileService)
        {
            this.userContext = userContext;
            this.mergeClient = mergeClient;
            this.blockchainService = blockchainService;
            this.virtualFileSystemFactory = virtualFileSystemFactory;
            this.playButtonPlugs = playButtonPlugs;
            this.searchService = searchService;
            this.warehouseClient = warehouseClient;
            this.fileClient = fileClient;
            this.dataTransferManager = dataTransferManager;
            this.profileService = profileService;

            CurrentUserDirectoryViewModel = new CurrentUserDirectoryViewModel(userContext.User, blockchainService, virtualFileSystemFactory, modalNavigationService, notificationsManager, playButtonPlugs);
            DirectoryTabs = new ObservableCollection<DirectoryTabViewModel>(new List<DirectoryTabViewModel> { CurrentUserDirectoryViewModel });
        }

        public DirectoryTabViewModel Content => SelectedIndex < 0 ? null : DirectoryTabs[SelectedIndex];
        public CurrentUserDirectoryViewModel CurrentUserDirectoryViewModel { get; private set; }
        public ObservableCollection<DirectoryTabViewModel> DirectoryTabs { get; set; }
        public Action Navigate { get; set; }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(Content));
            }
        }

        public async Task AddMergedTab(byte[] hash)
        {
            var searchResult = await GetAndStructureFilesPerNode(hash, mergeClient);
            var tab = new UserDirectoryViewModel(null, Convert.ToHexString(hash), searchResult, CreateResultsSnapshot, CloseTab, virtualFileSystemFactory, playButtonPlugs);
            DirectoryTabs.Add(tab);

            ChangeTabSelection(tab);
        }

        public async Task AddTab(string title, SearchResult searchResult)
        {
            var tab = new UserDirectoryViewModel(null, title, searchResult, CreateResultsSnapshot, CloseTab, virtualFileSystemFactory, playButtonPlugs);
            DirectoryTabs.Add(tab);

            ChangeTabSelection(tab);
        }

        public async Task AddTab(byte[] node)
        {
            DirectoryTabViewModel tab = await FindTab(Convert.ToHexString(node));

            if (tab == null)
            {
                var user = await profileService.GetUser(node);
                var searchResult = await blockchainService.GetFilesForNode(node);
                tab = new UserDirectoryViewModel(user, Convert.ToHexString(node), searchResult, CreateResultsSnapshot, CloseTab, virtualFileSystemFactory, playButtonPlugs);
                DirectoryTabs.Add(tab);
            }

            ChangeTabSelection(tab);
        }

        public Task CloseTab(DirectoryTabViewModel tab)
        {
            DirectoryTabs.Remove(tab);

            return Task.CompletedTask;
        }

        public override int GetNavigationIndex() => 2;

        private static async Task<SearchResult> GetAndStructureFilesPerNode(byte[] hash, IMergeClient mergeClient)
        {
            var result = await mergeClient.GetDirectoryContent(hash);
            result.Files?.ForEach(file =>
            {
                file.Folder = $"{Convert.ToHexString(file.NodeId)}/{file.Folder}";
            });

            return result;
        }

        private void ChangeTabSelection(DirectoryTabViewModel tab)
        {
            SelectedIndex = DirectoryTabs.IndexOf(tab);
        }

        private async Task<FileModel> CreateResultsSnapshot(string name, SearchResult searchResult)
        {
            var searchResultModel = new SearchResultModel();
            searchResultModel.Id = name;
            searchResultModel.Snapshot = JsonConvert.SerializeObject(searchResult);
            searchResultModel.Status = searchResult.Status;
            if (!searchResult.Files.IsNullOrEmpty())
            {
                searchResultModel.Rows = searchResult.Files.Select(f => new DownloadModel(f)).ToList();
            }

            var snapshot = new ResultsSnapshot
            {
                Title = name,
                SearchResultModel = searchResultModel,
                PeernetSchemaViewType = PeernetSchemaViewType.Directory
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
                await CurrentUserDirectoryViewModel.ReloadVirtualFileSystem();
            }

            return fileModel;
        }

        private async Task<DirectoryTabViewModel?> FindTab(string hexNode)
        {
            var currentUserPeerId = userContext.NodeId;
            if (string.Equals(hexNode, currentUserPeerId, StringComparison.OrdinalIgnoreCase))
            {
                return CurrentUserDirectoryViewModel;
            }

            return DirectoryTabs.FirstOrDefault(t => t.Identifier == hexNode);
        }
    }
}