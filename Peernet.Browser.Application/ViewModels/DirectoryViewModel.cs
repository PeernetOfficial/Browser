using DevExpress.Mvvm.Native;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : ViewModelBase
    {
        public ObservableCollection<DirectoryTabViewModel> DirectoryTabs { get; set; }

        private readonly IBlockchainService blockchainService;
        private readonly IUserContext userContext;
        private readonly IModalNavigationService modalNavigationService;
        private readonly INotificationsManager notificationsManager;
        private readonly IEnumerable<IPlayButtonPlug> playButtonPlugs;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private int selectedIndex = 0;

        public CurrentUserDirectoryViewModel CurrentUserDirectoryViewModel { get; private set; }

        public DirectoryTabViewModel Content => SelectedIndex < 0 ? null : DirectoryTabs[SelectedIndex];

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

        public DirectoryViewModel(
            IUserContext userContext,
            IBlockchainService blockchainService,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IEnumerable<IPlayButtonPlug> playButtonPlugs)
        {
            this.userContext = userContext;
            this.blockchainService = blockchainService;
            this.virtualFileSystemFactory = virtualFileSystemFactory;
            this.modalNavigationService = modalNavigationService;
            this.notificationsManager = notificationsManager;
            this.playButtonPlugs = playButtonPlugs;

            CurrentUserDirectoryViewModel = new CurrentUserDirectoryViewModel(blockchainService, CloseTab, virtualFileSystemFactory, modalNavigationService, notificationsManager, playButtonPlugs);
            DirectoryTabs = new ObservableCollection<DirectoryTabViewModel>(new List<DirectoryTabViewModel> { CurrentUserDirectoryViewModel });
        }

        public async Task AddTab(byte[] node)
        {
            DirectoryTabViewModel tab = await FindTab(node);

            if (tab == null)
            {
                tab = new UserDirectoryViewModel(node, blockchainService, CloseTab, virtualFileSystemFactory, modalNavigationService, notificationsManager, playButtonPlugs);
                DirectoryTabs.Add(tab);
            }

            ChangeTabSelection(tab);
        }

        private async Task<DirectoryTabViewModel?> FindTab(byte[] node)
        {
            var hexNode = Convert.ToHexString(node);
            var currentUserPeerId = userContext.NodeId;
            if (string.Equals(hexNode, currentUserPeerId, StringComparison.OrdinalIgnoreCase))
            {
                return CurrentUserDirectoryViewModel;
            }

            return DirectoryTabs.FirstOrDefault(t => t.Title == hexNode);
        }

        public Task CloseTab(DirectoryTabViewModel tab)
        {
            DirectoryTabs.Remove(tab);

            return Task.CompletedTask;
        }

        private void ChangeTabSelection(DirectoryTabViewModel tab)
        {
            SelectedIndex = DirectoryTabs.IndexOf(tab);
        }
    }
}