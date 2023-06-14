using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Profile;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public abstract class DirectoryTabViewModel : ViewModelBase
    {
        private const string LibrariesSegment = "Libraries";
        private const string YourFilesSegment = "Files";
        private readonly IEnumerable<IPlayButtonPlug> playButtonPlugs;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private ObservableCollection<VirtualFileSystemEntity> activeSearchResults;
        private bool isLoaded = false;
        private ObservableCollection<VirtualFileSystemCoreEntity> pathElements;
        private string identifier;
        private User user;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;

        public DirectoryTabViewModel(
            User? user,
            string identifier,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IEnumerable<IPlayButtonPlug> playButtonPlugs)
        {
            User = user;
            Identifier = identifier;
            this.virtualFileSystemFactory = virtualFileSystemFactory;
            this.playButtonPlugs = playButtonPlugs;
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

        protected List<ApiFile> Files { get; set; }

        public abstract bool IsReadOnly { get; }

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

        public IAsyncCommand SearchCommand =>
            new AsyncCommand(async () =>
            {
                await UpdateActiveSearchResults.ExecuteAsync(PathElements?.Last());
            });

        public IAsyncCommand<VirtualFileSystemEntity> StreamFileCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(
                entity =>
                {
                    playButtonPlugs.Foreach(async plug =>
                    {
                        if (plug?.IsSupported(entity.File) == true)
                        {
                            await plug?.Execute(entity.File);
                        }
                    });

                    return Task.CompletedTask;
                });

        public string Title => User?.Name ?? Identifier;

        public string Identifier
        {
            get => identifier;
            set
            {
                identifier = value;
                OnPropertyChanged(nameof(Identifier));
                OnPropertyChanged(nameof(Title));
            }
        }


        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged(nameof(User));
                OnPropertyChanged(nameof(Title));
            }
        }

        public IAsyncCommand<VirtualFileSystemCoreEntity> UpdateActiveSearchResults =>
            new AsyncCommand<VirtualFileSystemCoreEntity>(
                entity =>
                {
                    if (entity != null)
                    {
                        var refreshedEntity = VirtualFileSystem.Find(entity, VirtualFileSystem.VirtualFileSystemTiers) ?? VirtualFileSystem.Find(entity, VirtualFileSystem.VirtualFileSystemCategories);
                        ActiveSearchResults = new(refreshedEntity?.VirtualFileSystemEntities);
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

        protected void CreateVirtualFileSystem(bool restoreState = true)
        {
            var sharedFiles = (Files?.Where(f => f.Format != HighLevelFileType.PeernetSearch).ToList() ?? new()).Select(f => new VirtualFileSystemEntity(f)).ToList();
            SetPlayerState(sharedFiles);
            var selected = restoreState ? VirtualFileSystem?.GetCurrentlySelected() : null;

            VirtualFileSystem = virtualFileSystemFactory.CreateVirtualFileSystem(Files, selected?.Name == nameof(VirtualFileSystem.Home));
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
    }
}