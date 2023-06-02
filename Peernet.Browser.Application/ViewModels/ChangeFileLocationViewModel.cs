using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class ChangeFileLocationViewModel : ViewModelBase
    {
        private ObservableCollection<VirtualFileSystemEntity> activeSearchResults;
        private FileModel model;
        private ObservableCollection<VirtualFileSystemCoreEntity> pathElements;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;

        public ChangeFileLocationViewModel(VirtualFileSystem.VirtualFileSystem virtualFileSystem, string currentLocation, FileModel model)
        {
            VirtualFileSystem = virtualFileSystem;
            PathElements = new(ParseIntoPath(currentLocation));
            ActiveSearchResults = virtualFileSystem.GetCurrentlySelected().VirtualFileSystemEntities;
            this.model = model;
        }

        private List<VirtualFileSystemCoreEntity> ParseIntoPath(string currentLocation)
        {
            var result = new List<VirtualFileSystemCoreEntity>();
            List<VirtualFileSystemCoreTier> set = null;

            foreach (var segment in currentLocation.Split("\\"))
            {
                if (segment == "Files")
                {
                    continue;
                }
                else if (segment == nameof(VirtualFileSystem.Home))
                {
                    result.Add(VirtualFileSystem.Home);
                    set = VirtualFileSystem.Home.SubTiers;
                }
                else
                {
                    var matching = set?.FirstOrDefault(s => s?.Name == segment);
                    result.Add(matching);
                    set = matching.SubTiers;
                }
            }

            return result;
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

        public IAsyncCommand CreateNewFolderCommand =>
            new AsyncCommand(() =>
            {
                string defaultFolderName = "New Folder";
                var selected = virtualFileSystem.GetCurrentlySelected();
                virtualFileSystem.GetCurrentlySelected().VirtualFileSystemEntities.Add(new VirtualFileSystemCoreTier(defaultFolderName, VirtualFileSystemEntityType.Directory, selected.AbsolutePath));
                ActiveSearchResults = new(selected.VirtualFileSystemEntities);

                return Task.CompletedTask;
            });

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

        public IAsyncCommand<VirtualFileSystemEntity> OpenTreeFolderCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(async coreEntity =>
            {
                if (coreEntity is VirtualFileSystemCoreTier coreTier)
                {
                    InitializePath(virtualFileSystem?.Home);
                    await OpenCommand.ExecuteAsync(coreTier);
                }
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

        public IAsyncCommand SelectCommand =>
            new AsyncCommand(() =>
            {
                var selected = virtualFileSystem.GetCurrentlySelected();
                model.Directory = TrimUnsopportedSegments(selected.AbsolutePath);

                return Task.CompletedTask;
            });

        public IAsyncCommand<VirtualFileSystemCoreEntity> UpdateActiveSearchResults =>
            new AsyncCommand<VirtualFileSystemCoreEntity>(
                entity =>
                {
                    if (entity != null)
                    {
                        ActiveSearchResults = ApplySearchResultsFiltering(entity?.VirtualFileSystemEntities.ToList());
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

        public static string TrimUnsopportedSegments(string path)
        {
            return path.Replace("Files\\", string.Empty).Replace("Home", string.Empty).Replace("\\\\", "\\").Trim('\\');
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

            PathElements = new ObservableCollection<VirtualFileSystemCoreEntity>(
                new List<VirtualFileSystemCoreEntity>
                    { VirtualFileSystem.Home });
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

        private ObservableCollection<VirtualFileSystemEntity> ApplySearchResultsFiltering(List<VirtualFileSystemEntity> results)
        {
            if (results.IsNullOrEmpty())
            {
                return new ObservableCollection<VirtualFileSystemEntity>();
            }

            return new(results);
        }
    }
}