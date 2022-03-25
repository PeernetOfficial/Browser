using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : ViewModelBase, ISearchable
    {
        private const string LibrariesSegment = "Libraries";
        private const string YourFilesSegment = "Your Files";
        private readonly IBlockchainService blockchainService;
        private readonly IModalNavigationService modalNavigationService;
        private readonly INotificationsManager notificationsManager;
        private readonly IEnumerable<IPlayButtonPlug> playButtonPlugs;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private ObservableCollection<VirtualFileSystemEntity> activeSearchResults;
        private bool isLoaded = false;
        private ObservableCollection<VirtualFileSystemCoreEntity> pathElements;
        private string searchInput;
        private bool showActionsColumn = true;
        private bool showDataFormatColumn = false;
        private bool showDateColumn = true;
        private bool showFolderColumn = false;
        private bool showHint = true;
        private bool showSearchBox;
        private bool showSizeColumn = true;
        private bool showTypeColumn = true;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;

        public DirectoryViewModel(
            IBlockchainService blockchainService,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IModalNavigationService modalNavigationService,
            INotificationsManager notificationsManager,
            IEnumerable<IPlayButtonPlug> playButtonPlugs)
        {
            this.blockchainService = blockchainService;
            this.virtualFileSystemFactory = virtualFileSystemFactory;
            this.modalNavigationService = modalNavigationService;
            this.notificationsManager = notificationsManager;
            this.playButtonPlugs = playButtonPlugs;

            SetupColumnsFilter();
            Task.Run(async () => await ReloadVirtualFileSystem(false)).ConfigureAwait(false).GetAwaiter().GetResult();
            InitializePath(VirtualFileSystem?.Home);
            OpenCommand.Execute(VirtualFileSystem?.Home);
        }

        public ObservableCollection<VirtualFileSystemEntity> ActiveSearchResults
        {
            get => new(activeSearchResults?.OrderBy(e => (int)e.Type)?.ToList() ?? Enumerable.Empty<VirtualFileSystemEntity>());
            set
            {
                activeSearchResults = value;
                OnPropertyChanged(nameof(ActiveSearchResults));
            }
        }

        public ObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new ObservableCollection<CustomCheckBoxModel>();

        public IconModel ColumnsIconModel { get; } = new IconModel(FilterType.Columns, true);

        public IAsyncCommand<VirtualFileSystemEntity> DeleteCommand =>
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

        public bool IsLoaded
        {
            get => isLoaded;
            set
            {
                isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        public IAsyncCommand<VirtualFileSystemEntity> OpenCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(
                async entity =>
                {
                    if (entity == null)
                    {
                        return;
                    }

                    if (entity is VirtualFileSystemCoreEntity coreTier)
                    {
                        await UpdateActiveSearchResults.ExecuteAsync(coreTier);
                        SetPath(coreTier);
                    }
                });

        public IAsyncCommand<VirtualFileSystemCoreEntity> OpenTreeItemCommand =>
            new AsyncCommand<VirtualFileSystemCoreEntity>(
                async entity =>
                {
                    InitializePath(entity);
                    await OpenCommand.ExecuteAsync(entity);
                });

        public ObservableCollection<VirtualFileSystemCoreEntity> PathElements
        {
            get => pathElements;
            set
            {
                pathElements = value;
                OnPropertyChanged(nameof(PathElements));
            }
        }

        public IAsyncCommand RemoveHint => new AsyncCommand(() =>
                       {
                           if (ShowHint)
                           {
                               ShowHint = false;
                               ShowSearchBox = true;
                           }

                           return Task.CompletedTask;
                       });

        public IAsyncCommand SearchCommand =>
            new AsyncCommand(async () =>
            {
                await UpdateActiveSearchResults.ExecuteAsync(PathElements?.Last());
            });

        public string SearchInput
        {
            get => searchInput;
            set
            {
                searchInput = value;
                OnPropertyChanged(nameof(SearchInput));
            }
        }

        public bool ShowActionsColumn
        {
            get => showActionsColumn;
            set
            {
                showActionsColumn = value;
                OnPropertyChanged(nameof(ShowActionsColumn));
            }
        }

        public bool ShowDataFormatColumn
        {
            get => showDataFormatColumn;
            set
            {
                showDataFormatColumn = value;
                OnPropertyChanged(nameof(ShowDataFormatColumn));
            }
        }

        public bool ShowDateColumn
        {
            get => showDateColumn;
            set
            {
                showDateColumn = value;
                OnPropertyChanged(nameof(ShowDateColumn));
            }
        }

        public bool ShowFolderColumn
        {
            get => showFolderColumn;
            set
            {
                showFolderColumn = value;
                OnPropertyChanged(nameof(ShowFolderColumn));
            }
        }

        public bool ShowHint
        {
            get => showHint;
            set
            {
                showHint = value;
                OnPropertyChanged(nameof(ShowHint));
            }
        }

        public bool ShowSearchBox
        {
            get => showSearchBox;
            set
            {
                showSearchBox = value;
                OnPropertyChanged(nameof(ShowSearchBox));
            }
        }

        public bool ShowSizeColumn
        {
            get => showSizeColumn;
            set
            {
                showSizeColumn = value;
                OnPropertyChanged(nameof(ShowSizeColumn));
            }
        }

        public bool ShowTypeColumn
        {
            get => showTypeColumn;
            set
            {
                showTypeColumn = value;
                OnPropertyChanged(nameof(ShowTypeColumn));
            }
        }

        public IAsyncCommand<VirtualFileSystemEntity> StreamFileCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(
                entity =>
                {
                    playButtonPlugs.Foreach(plug =>
                    {
                        if (plug?.IsSupported(entity.File) == true)
                        {
                            plug?.Execute(entity.File);
                        }
                    });

                    return Task.CompletedTask;
                });

        public IAsyncCommand<VirtualFileSystemCoreEntity> UpdateActiveSearchResults =>
            new AsyncCommand<VirtualFileSystemCoreEntity>(
                entity =>
                {
                    if (entity != null)
                    {
                        var refreshedEntity = VirtualFileSystem.Find(entity, VirtualFileSystem.VirtualFileSystemTiers) ?? VirtualFileSystem.Find(entity, VirtualFileSystem.VirtualFileSystemCategories);
                        ActiveSearchResults = ApplySearchResultsFiltering(refreshedEntity?.VirtualFileSystemEntities.ToList());
                    }

                    return Task.CompletedTask;
                });

        public VirtualFileSystem.VirtualFileSystem VirtualFileSystem
        {
            get => virtualFileSystem;
            set
            {
                virtualFileSystem = value;
                OnPropertyChanged(nameof(VirtualFileSystem));
            }
        }

        public void ChangeSelectedEntity(VirtualFileSystemCoreEntity coreEntity)
        {
            VirtualFileSystem.ResetSelection();
            coreEntity.IsSelected = true;
        }

        public void InitializePath(VirtualFileSystemEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            var name = entity is VirtualFileSystemCoreCategory ? LibrariesSegment : YourFilesSegment;

            PathElements = new ObservableCollection<VirtualFileSystemCoreEntity>(
                new List<VirtualFileSystemCoreEntity>
                    { new(name, VirtualFileSystemEntityType.Directory) });
        }

        public async Task ReloadVirtualFileSystem(bool restoreState = true)
        {
            var header = await blockchainService.GetHeader();
            if (header == null)
            {
                return;
            }

            IsLoaded = false;
            var files = await blockchainService.GetList();

            var sharedFiles = (files ?? new()).Select(f => new VirtualFileSystemEntity(f)).ToList();
            SetPlayerState(sharedFiles);
            var selected = restoreState ? VirtualFileSystem?.GetCurrentlySelected() : null;

            VirtualFileSystem = virtualFileSystemFactory.CreateVirtualFileSystem(files, selected?.Name == nameof(VirtualFileSystem.Home));
            SetPlayerStateRecursively(VirtualFileSystem.Home.VirtualFileSystemEntities.ToList());
            VirtualFileSystem.VirtualFileSystemCategories.Foreach(c => SetPlayerState(c.VirtualFileSystemEntities.ToList()));
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
                    OpenCommand.Execute(DetermineHigherTier() ?? VirtualFileSystem.Home);
                }
            }

            IsLoaded = true;
        }

        public void SetPath(VirtualFileSystemCoreEntity entity)
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

            ChangeSelectedEntity(PathElements.Last());
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
            entities.Foreach(e => tier.VirtualFileSystemEntities.Add(e));
            VirtualFileSystem.VirtualFileSystemTiers.Add(tier);
        }

        private ObservableCollection<VirtualFileSystemEntity> ApplySearchResultsFiltering(List<VirtualFileSystemEntity> results)
        {
            if (results.IsNullOrEmpty())
            {
                return new ObservableCollection<VirtualFileSystemEntity>();
            }

            return !string.IsNullOrEmpty(SearchInput)
                ? new(results.Where(f => f.Name.Contains(SearchInput, StringComparison.OrdinalIgnoreCase)).ToList())
                : new(results);
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

        private void OnColumnCheckboxClick(CustomCheckBoxModel selection)
        {
            switch (selection.Content)
            {
                case "Folder":
                    ShowFolderColumn = selection.IsChecked;
                    break;

                case "Type":
                    ShowTypeColumn = selection.IsChecked;
                    break;

                case "Data Format":
                    ShowDataFormatColumn = selection.IsChecked;
                    break;

                case "Date":
                    ShowDateColumn = selection.IsChecked;
                    break;

                case "Size":
                    ShowSizeColumn = selection.IsChecked;
                    break;

                case "Actions":
                    ShowActionsColumn = selection.IsChecked;
                    break;
            }
        }

        private void RefreshPathObjects()
        {
            List<VirtualFileSystemCoreEntity> refreshedPath = new();
            if (PathElements != null)
            {
                foreach (var element in PathElements)
                {
                    if (element == null)
                    {
                        continue;
                    }

                    if (element.Name is YourFilesSegment or LibrariesSegment)
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

        private void SetPlayerState(List<VirtualFileSystemEntity> results)
        {
            results.Foreach(r =>
            {
                r.IsPlayerEnabled = playButtonPlugs.Any(plug => plug?.IsSupported(r.File) == true);
            });
        }

        private void SetPlayerStateRecursively(List<VirtualFileSystemEntity> results)
        {
            SetPlayerState(results);
            results.Foreach(r =>
            {
                if (r is VirtualFileSystemCoreEntity coreEntity)
                {
                    SetPlayerStateRecursively(coreEntity.VirtualFileSystemEntities.ToList());
                }
            });
        }

        private void SetupColumnsFilter()
        {
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Folder", IsChecked = showFolderColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Data Format", IsChecked = showDataFormatColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Type", IsChecked = showTypeColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Date", IsChecked = showDateColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Size", IsChecked = showSizeColumn, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Actions", IsChecked = showActionsColumn, IsCheckChanged = OnColumnCheckboxClick });
        }
    }
}