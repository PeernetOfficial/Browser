using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.VirtualFileSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class ChangeFileLocationViewModel : ViewModelBase
    {
        private ObservableCollection<VirtualFileSystemEntity> activeSearchResults;
        private ObservableCollection<VirtualFileSystemCoreEntity> pathElements;
        private Action<string> updatePathAction;

        public ChangeFileLocationViewModel(VirtualFileSystem.VirtualFileSystem virtualFileSystem, Action<string> updatePathAction)
        {
            VirtualFileSystem = virtualFileSystem;
            this.updatePathAction = updatePathAction;
            ActiveSearchResults = new(VirtualFileSystem.Home.VirtualFileSystemEntities);
            ChangeSelection(VirtualFileSystem.Home);
            ResetPath();
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
                var selected = VirtualFileSystem.GetCurrentlySelected();
                VirtualFileSystem.GetCurrentlySelected().VirtualFileSystemEntities.Add(new VirtualFileSystemCoreTier(defaultFolderName, VirtualFileSystemEntityType.Directory, selected.AbsolutePath));

                ActiveSearchResults = new(selected.VirtualFileSystemEntities);

                return Task.CompletedTask;
            });

        public IAsyncCommand<VirtualFileSystemEntity> OpenFolderCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(coreEntity =>
            {
                if (coreEntity is VirtualFileSystemCoreTier coreTier)
                {
                    ActiveSearchResults = new(coreTier.VirtualFileSystemEntities);
                    ChangeSelection(coreTier);
                    SetPath(coreTier);
                }

                return Task.CompletedTask;
            });

        public IAsyncCommand<VirtualFileSystemEntity> OpenTreeFolderCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(coreEntity =>
            {
                if (coreEntity is VirtualFileSystemCoreTier coreTier)
                {
                    ActiveSearchResults = new(coreTier.VirtualFileSystemEntities);
                    ChangeSelection(coreTier);
                    ResetPath();
                    SetPath(coreTier);
                }

                return Task.CompletedTask;
            });

        public IAsyncCommand SelectCommand =>
            new AsyncCommand(() =>
            {
                var selected = VirtualFileSystem.GetCurrentlySelected();
                updatePathAction(selected.AbsolutePath.Replace("Your Files\\", string.Empty));

                return Task.CompletedTask;
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

        public VirtualFileSystem.VirtualFileSystem VirtualFileSystem { get; set; }

        private void ChangeSelection(VirtualFileSystemCoreTier coreTier)
        {
            VirtualFileSystem.ResetSelection();
            coreTier.IsSelected = true;
        }

        private void ResetPath()
        {
            PathElements = new ObservableCollection<VirtualFileSystemCoreEntity>(
                new List<VirtualFileSystemCoreEntity>
                {
                    new(nameof(VirtualFileSystem.Home), VirtualFileSystemEntityType.Directory, nameof(VirtualFileSystem.Home))
                });
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
    }
}