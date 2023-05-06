using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class UserDirectoryViewModel : DirectoryTabViewModel
    {
        public UserDirectoryViewModel(
            byte[] node,
            IBlockchainService blockchainService,
            Func<DirectoryTabViewModel, Task> removeTabAction,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IEnumerable<IPlayButtonPlug> playButtonPlugs)
            : base(
                  Convert.ToHexString(node),
                  async () => await blockchainService.GetFilesForNode(node),
                  blockchainService,
                  virtualFileSystemFactory,
                  modalNavigationService,
                  notificationsManager,
                  playButtonPlugs)
        {
            DeleteCommand = new AsyncCommand(async () => await removeTabAction(this));
        }

        public IAsyncCommand DeleteCommand { get; }
    }
}