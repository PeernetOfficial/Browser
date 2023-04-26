using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class UserMergedDirectoryViewModel : DirectoryTabViewModel
    {
        public UserMergedDirectoryViewModel(
            byte[] hash,
            IMergeClient mergeClient,
            IBlockchainService blockchainService,
            Func<DirectoryTabViewModel, Task> removeTabAction,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IEnumerable<IPlayButtonPlug> playButtonPlugs)
            : base(
                  Convert.ToHexString(hash),
                  async () => await GetAndStructureFilesPerNode(hash, mergeClient),
                  blockchainService,
                  virtualFileSystemFactory,
                  modalNavigationService,
                  notificationsManager,
                  playButtonPlugs)
        {
            DeleteCommand = new AsyncCommand(async () => await removeTabAction(this));
        }

        public IAsyncCommand DeleteCommand { get; }

        private static async Task<List<ApiFile>> GetAndStructureFilesPerNode(byte[] hash, IMergeClient mergeClient)
        {
            var result = await mergeClient.GetDirectoryContent(hash);
            result.Files.ForEach(file =>
            {
                file.Folder = $"{Convert.ToHexString(file.NodeId)}/{file.Folder}"; 
            });

            return result.Files;
        }
    }
}
