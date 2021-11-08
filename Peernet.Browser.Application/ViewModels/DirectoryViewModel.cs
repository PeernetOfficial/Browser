using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Peernet.Browser.Application.ViewModels.Parameters;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : MvxViewModel, ISearchable
    {
        private readonly IBlockchainService blockchainService;
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private List<VirtualFileSystemEntity> activeSearchResults;
        private ObservableCollection<VirtualFileSystemCoreEntity> pathElements;
        private string searchInput;
        private IReadOnlyCollection<VirtualFileSystemEntity> sharedFiles;
        private bool showHint = true;
        private bool showSearchBox;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;

        public DirectoryViewModel(IBlockchainService blockchainService, IVirtualFileSystemFactory virtualFileSystemFactory, IMvxNavigationService mvxNavigationService)
        {
            this.blockchainService = blockchainService;
            this.virtualFileSystemFactory = virtualFileSystemFactory;
            this.mvxNavigationService = mvxNavigationService;
        }

        public List<VirtualFileSystemEntity> ActiveSearchResults
        {
            get => activeSearchResults?.OrderBy(e=> (int)e.Type).ToList();
            set => SetProperty(ref activeSearchResults, value);
        }

        public IMvxCommand<VirtualFileSystemEntity> OpenCommand =>
            new MvxCommand<VirtualFileSystemEntity>(
                entity =>
                {
                    if (entity is VirtualFileSystemCoreTier coreTier)
                    {
                        ActiveSearchResults = coreTier.VirtualFileSystemEntities;
                    }
                    else
                    {
                        var param = new FilePreviewViewModelParameter(entity.File, false, true, "Save To File");
                        mvxNavigationService.Navigate<FilePreviewViewModel, FilePreviewViewModelParameter>(param);
                    }
                });

        public IMvxAsyncCommand<VirtualFileSystemEntity> DeleteCommand =>
            new MvxAsyncCommand<VirtualFileSystemEntity>(
                async entity =>
                {
                    await blockchainService.DeleteFile(entity.File);
                    await ReloadVirtualFileSystem();
                });

        public IMvxAsyncCommand<VirtualFileSystemEntity> EditCommand =>
                    new MvxAsyncCommand<VirtualFileSystemEntity>(
                 async entity =>
                {
                    var parameter = new EditFileViewModelParameter(blockchainService, mvxNavigationService)
                    {
                        FileModels = new FileModel[]
                        {
                            new(entity.File)
                        }
                    };

                    GlobalContext.IsMainWindowActive = false;
                    await mvxNavigationService.Navigate<GenericFileViewModel, EditFileViewModelParameter>(parameter);
                });

        public ObservableCollection<VirtualFileSystemCoreEntity> PathElements
        {
            get => pathElements;
            set => SetProperty(ref pathElements, value);
        }

        public IMvxCommand RemoveHint
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (ShowHint)
                    {
                        ShowHint = false;
                        ShowSearchBox = true;
                    }
                });
            }
        }

        public IMvxCommand SearchCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (sharedFiles != null)
                    {
                        ActiveSearchResults = ApplySearchResultsFiltering(sharedFiles.ToList());
                    }
                });
            }
        }

        public string SearchInput
        {
            get => searchInput;
            set => SetProperty(ref searchInput, value);
        }

        public bool ShowHint
        {
            get => showHint;
            set => SetProperty(ref showHint, value);
        }

        public bool ShowSearchBox
        {
            get => showSearchBox;
            set => SetProperty(ref showSearchBox, value);
        }

        public IMvxCommand<VirtualFileSystemCoreTier> UpdateActiveSearchResults =>
            new MvxCommand<VirtualFileSystemCoreTier>(
                tier => { ActiveSearchResults = ApplySearchResultsFiltering(tier.VirtualFileSystemEntities); });

        public VirtualFileSystem.VirtualFileSystem VirtualFileSystem
        {
            get => virtualFileSystem;
            set => SetProperty(ref virtualFileSystem, value);
        }

        public void ChangeSelectedEntity(VirtualFileSystemCoreEntity coreEntity)
        {
            VirtualFileSystem.ResetSelection();
            coreEntity.IsSelected = true;
        }

        public override async void ViewAppearing()
        {
            await ReloadVirtualFileSystem();
            base.ViewAppearing();
        }

        private void AddAllFilesTier(IEnumerable<VirtualFileSystemEntity> entities)
        {
            AddTier("All files", VirtualFileSystemEntityType.All, entities);
        }

        private void AddRecentTier(IEnumerable<VirtualFileSystemEntity> entities)
        {
            var filtered = entities.OrderByDescending(f => f.File.Date).Take(10);
            AddTier("Recent", VirtualFileSystemEntityType.Recent, filtered);
        }

        private void AddTier(string name, VirtualFileSystemEntityType type, IEnumerable<VirtualFileSystemEntity> entities)
        {
            var tier = new VirtualFileSystemCoreTier(name, type);
            tier.VirtualFileSystemEntities.AddRange(entities);

            VirtualFileSystem.VirtualFileSystemTiers.Add(tier);
        }

        private List<VirtualFileSystemEntity> ApplySearchResultsFiltering(List<VirtualFileSystemEntity> results)
        {
            return !string.IsNullOrEmpty(SearchInput)
                ? results.Where(f => f.Name.Contains(SearchInput, StringComparison.OrdinalIgnoreCase)).ToList()
                : results.ToList();
        }

        private async Task ReloadVirtualFileSystem()
        {
            var header = await blockchainService.GetHeader();
            var files = await blockchainService.GetList();
            if (header.Height > 0 || header.Height != ActiveSearchResults?.Count)
            {
                sharedFiles = (files ?? new()).Select(f => new VirtualFileSystemEntity(f)).ToList();
                ActiveSearchResults = sharedFiles?.ToList();
            }

            VirtualFileSystem = virtualFileSystemFactory.CreateVirtualFileSystem(files);
            AddRecentTier(sharedFiles);
            AddAllFilesTier(sharedFiles);
        }
    }
}