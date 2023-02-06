using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class UserDirectoryViewModel : DirectoryTabViewModel
    {
        private static string NodeId;

        public UserDirectoryViewModel(
            string nodeId,
            IBlockchainService blockchainService,
            Func<DirectoryTabViewModel, Task> removeTabAction,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IEnumerable<IPlayButtonPlug> playButtonPlugs)
            : base(nodeId, GetFiles, blockchainService, virtualFileSystemFactory, modalNavigationService, notificationsManager, playButtonPlugs)
        {
            NodeId = nodeId;
            DeleteCommand = new AsyncCommand(async () => await removeTabAction(this));

        }

        private static async Task<List<ApiFile>> GetFiles(IBlockchainService blockchainService)
        {
            return await blockchainService.GetFilesForNode(NodeId);
        }

        public IAsyncCommand DeleteCommand { get; }

    }
}