using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Profile;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class CurrentUserDirectoryViewModel : DirectoryTabViewModel
    {
        private const string TabIdentifier = "My Directory";
        private readonly IBlockchainService blockchainService;
        private readonly INotificationsManager notificationsManager;
        private readonly IModalNavigationService modalNavigationService;
        private Func<Task<List<ApiFile>>> filesProvider;

        public CurrentUserDirectoryViewModel(
            User user,
            IBlockchainService blockchainService,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IEnumerable<IPlayButtonPlug> playButtonPlugs)
            : base(user, TabIdentifier, virtualFileSystemFactory, playButtonPlugs)
        {
            this.blockchainService = blockchainService;
            this.modalNavigationService = modalNavigationService;
            this.notificationsManager = notificationsManager;
            Title = TabIdentifier;
            filesProvider = async () => await GetFiles(blockchainService);

            Task.Run(async () => await ReloadVirtualFileSystem(false)).ConfigureAwait(false).GetAwaiter().GetResult();
            InitializePath(VirtualFileSystem?.Home);
            OpenCommand.Execute(VirtualFileSystem?.Home);
        }

        public IAsyncCommand<VirtualFileSystemEntity> DeleteFileCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(
                async entity =>
                {
                    var result = await blockchainService.DeleteFile(entity.File);
                    if (result.Status != BlockchainStatus.StatusOK)
                    {
                        var logMessage = $"Failed to delete file. Status: {result.Status}";
                        var details =
                            MessagingHelper.GetApiSummary($"{nameof(blockchainService)}.{nameof(blockchainService.DeleteFile)}") +
                            MessagingHelper.GetInOutSummary(entity.File, result);
                        notificationsManager.Notifications.Add(new Notification(logMessage, details, Severity.Error));
                        return;
                    }

                    await ReloadVirtualFileSystem();
                });

        public IAsyncCommand<VirtualFileSystemEntity> EditCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(
                entity =>
                {
                    var parameter = new EditFileViewModelParameter(blockchainService, notificationsManager, async () => await ReloadVirtualFileSystem())
                    {
                        FileModels = new List<FileModel>
                {
                            new(entity.File)
                }
                    };

                    modalNavigationService.Navigate<EditFileViewModel, EditFileViewModelParameter>(parameter);

                    return Task.CompletedTask;
                });

        public override bool IsReadOnly => false;

        public async Task ReloadVirtualFileSystem(bool restoreState = true)
        {
            IsLoaded = false;
            Files = await filesProvider();
            CreateVirtualFileSystem();
            IsLoaded = true;
        }

        private static async Task<List<ApiFile>> GetFiles(IBlockchainService blockchainService)
        {
            var header = await blockchainService.GetHeader();
            if (header == null)
            {
                return new();
            }

            return await blockchainService.GetList();
        }
    }
}