using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : MvxViewModel, ISearchable
    {
        private readonly IBlockchainService blockchainService;
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private readonly IWarehouseService warehouseService;
        private List<VirtualFileSystemEntity> activeSearchResults;
        private ObservableCollection<VirtualFileSystemCoreEntity> pathElements;
        private string searchInput;
        private IReadOnlyCollection<VirtualFileSystemEntity> sharedFiles;
        private bool showHint = true;
        private bool showSearchBox;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;

        public DirectoryViewModel(
            IBlockchainService blockchainService,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IMvxNavigationService mvxNavigationService,
            IWarehouseService warehouseService)
        {
            this.blockchainService = blockchainService;
            this.virtualFileSystemFactory = virtualFileSystemFactory;
            this.mvxNavigationService = mvxNavigationService;
            this.warehouseService = warehouseService;
        }

        public List<VirtualFileSystemEntity> ActiveSearchResults
        {
            get => activeSearchResults?.OrderBy(e => (int)e.Type).ToList();
            set
            {
                SetProperty(ref activeSearchResults, value);
                RaisePropertyChanged(nameof(ActiveSearchResults));
            }
        }

        public IMvxAsyncCommand<VirtualFileSystemEntity> DeleteCommand =>
            new MvxAsyncCommand<VirtualFileSystemEntity>(
                async entity =>
                {
                    var result = await blockchainService.DeleteFile(entity.File);
                    if (result.Status != BlockchainStatus.StatusOK)
                    {
                        GlobalContext.Notifications.Add(new Notification($"Failed to delete file. Status: {result.Status}", Severity.Error));
                        return;
                    }

                    await ReloadVirtualFileSystem();
                });

        public IMvxAsyncCommand<VirtualFileSystemEntity> EditCommand =>
                    new MvxAsyncCommand<VirtualFileSystemEntity>(
                 async entity =>
                {
                    var parameter = new EditFileViewModelParameter(blockchainService, mvxNavigationService)
                    {
                        FileModels = new List<FileModel>
                        {
                            new(entity.File)
                        }
                    };

                    GlobalContext.IsMainWindowActive = false;
                    await mvxNavigationService.Navigate<GenericFileViewModel, EditFileViewModelParameter>(parameter);
                });

        public IMvxCommand<VirtualFileSystemEntity> OpenCommand =>
            new MvxCommand<VirtualFileSystemEntity>(
                entity =>
                {
                    if (entity is VirtualFileSystemCoreEntity coreTier)
                    {
                        UpdateActiveSearchResults.Execute(coreTier);
                        SetPath(coreTier);
                        ChangeSelectedEntity(coreTier);
                    }
                    else
                    {
                        var param = new FilePreviewViewModelParameter(entity.File, false, () => warehouseService.ReadPath(entity.File), "Save To File");
                        mvxNavigationService.Navigate<FilePreviewViewModel, FilePreviewViewModelParameter>(param);
                    }
                });

        public IMvxCommand<VirtualFileSystemCoreEntity> OpenDirectoryCommand =>
            new MvxCommand<VirtualFileSystemCoreEntity>(entity =>
            {
                ChangeSelectedEntity(entity);
                SetPath(entity);
                UpdateActiveSearchResults.Execute(entity);
            });

        public IMvxCommand<VirtualFileSystemCoreEntity> OpenTreeItemCommand => new MvxCommand<VirtualFileSystemCoreEntity>(InitializePath);

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

        public IMvxCommand SearchCommand =>
            new MvxCommand(() =>
            {
                UpdateActiveSearchResults.Execute(PathElements.Last());
            });

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

        public IMvxCommand<VirtualFileSystemCoreEntity> UpdateActiveSearchResults =>
            new MvxCommand<VirtualFileSystemCoreEntity>(
                entity =>
                {
                    var refreshedEntity = VirtualFileSystem.Find(entity, VirtualFileSystem.VirtualFileSystemTiers) ?? VirtualFileSystem.Find(entity, VirtualFileSystem.VirtualFileSystemCategories);
                    ActiveSearchResults = ApplySearchResultsFiltering(refreshedEntity?.VirtualFileSystemEntities);
                });

        public VirtualFileSystem.VirtualFileSystem VirtualFileSystem
        {
            get => virtualFileSystem;
            set => SetProperty(ref virtualFileSystem, value);
        }

        public async Task ReloadVirtualFileSystem(bool restoreState = true)
        {
            var header = await blockchainService.GetHeader();
            var files = await blockchainService.GetList();
            if (header.Height > 0 || header.Height != ActiveSearchResults?.Count)
            {
                sharedFiles = (files ?? new()).Select(f => new VirtualFileSystemEntity(f)).ToList();
            }

            var selected = restoreState ? VirtualFileSystem?.GetCurrentlySelected() : null;

            VirtualFileSystem = virtualFileSystemFactory.CreateVirtualFileSystem(files, selected?.Name == "Root");
            AddRecentTier(sharedFiles);
            AddAllFilesTier(sharedFiles);
            RefreshPathObjects();
            if (selected != null)
            {
                VirtualFileSystemCoreEntity matchingEntity = null;
                if (VirtualFileSystem.VirtualFileSystemTiers != null && VirtualFileSystem.VirtualFileSystemCategories != null)
                {
                    matchingEntity =
                        VirtualFileSystem.Find(selected, VirtualFileSystem.VirtualFileSystemTiers) ?? VirtualFileSystem.Find(selected, VirtualFileSystem.VirtualFileSystemCategories);
                }

                if (matchingEntity != null)
                {
                    ChangeSelectedEntity(matchingEntity);
                    UpdateActiveSearchResults.Execute(matchingEntity);
                }
                else
                {
                    OpenDirectoryCommand.Execute(DetermineHigherTier() ?? VirtualFileSystem.Root);
                }
            }
        }

        public override async void ViewAppearing()
        {
            await ReloadVirtualFileSystem(false);
            InitializePath(VirtualFileSystem.Root);

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
            var tier = new VirtualFileSystemCoreTier(name, type, name);
            tier.VirtualFileSystemEntities.AddRange(entities);
            VirtualFileSystem.VirtualFileSystemTiers.Add(tier);
        }

        private List<VirtualFileSystemEntity> ApplySearchResultsFiltering(List<VirtualFileSystemEntity> results)
        {
            if (results.IsNullOrEmpty())
            {
                return new List<VirtualFileSystemEntity>();
            }

            return !string.IsNullOrEmpty(SearchInput)
                ? results.Where(f => f.Name.Contains(SearchInput, StringComparison.OrdinalIgnoreCase)).ToList()
                : results;
        }

        private void ChangeSelectedEntity(VirtualFileSystemCoreEntity coreEntity)
        {
            VirtualFileSystem.ResetSelection();
            coreEntity.IsSelected = true;
        }

        private VirtualFileSystemCoreEntity DetermineHigherTier()
        {
            for (int i = 1; i < PathElements.Count; i++)
            {
                var higherTier = PathElements.ElementAt(PathElements.Count - 1 - i);
                var matchingTier = VirtualFileSystem.Find(higherTier, VirtualFileSystem.VirtualFileSystemTiers);
                if (matchingTier != null && !matchingTier.VirtualFileSystemEntities.IsNullOrEmpty())
                {
                    return matchingTier;
                }
            }

            return null;
        }

        private void InitializePath(VirtualFileSystemEntity entity)
        {
            var name = entity is VirtualFileSystemCoreCategory ? "Libraries" : "Your Files";

            PathElements = new ObservableCollection<VirtualFileSystemCoreEntity>(
                new List<VirtualFileSystemCoreEntity>
                    { new(name, VirtualFileSystemEntityType.Directory, name) });

            OpenCommand.Execute(entity);
        }

        private void SetPath(VirtualFileSystemCoreEntity entity)
        {
            var index = PathElements.IndexOf(entity);
            if (index == -1)
            {
                PathElements.Add(entity);
            }
            else
            {
                for (int i = PathElements.Count - 1; i > index; i--)
                {
                    PathElements.RemoveAt(i);
                }
            }

            PathElements.Last().IsSelected = true;
        }

        private void RefreshPathObjects()
        {
            List<VirtualFileSystemCoreEntity> refreshedPath = new();
            if (PathElements != null)
            {
                foreach (var element in PathElements)
                {
                    if (element.Name is "Your Files" or "Libraries")
                    {
                        refreshedPath.Add(element);
                        continue;
                    }

                    refreshedPath.Add(VirtualFileSystem.Find(element, VirtualFileSystem.VirtualFileSystemTiers) ??
                                                 VirtualFileSystem.Find(element, VirtualFileSystem.VirtualFileSystemCategories));
                }
            }

            PathElements = new ObservableCollection<VirtualFileSystemCoreEntity>(refreshedPath);
        }
    }
}