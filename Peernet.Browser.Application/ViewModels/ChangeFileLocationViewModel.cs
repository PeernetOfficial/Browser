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

        public ChangeFileLocationViewModel(DirectoryViewModel directoryViewModel, Action<string> updatePathAction)
        {
            DirectoryViewModel = directoryViewModel;
            this.updatePathAction = updatePathAction;
        }

        public IAsyncCommand CreateNewFolderCommand =>
            new AsyncCommand(() =>
            {
                string defaultFolderName = "New Folder";
                var selected = DirectoryViewModel.VirtualFileSystem.GetCurrentlySelected();
                DirectoryViewModel.VirtualFileSystem.GetCurrentlySelected().VirtualFileSystemEntities.Add(new VirtualFileSystemCoreTier(defaultFolderName, VirtualFileSystemEntityType.Directory, selected.AbsolutePath));
                DirectoryViewModel.ActiveSearchResults = new(selected.VirtualFileSystemEntities);

                return Task.CompletedTask;
            });

        public IAsyncCommand<VirtualFileSystemEntity> OpenFolderCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(coreEntity =>
            {
                if (coreEntity is VirtualFileSystemCoreTier coreTier)
                {
                    DirectoryViewModel.ActiveSearchResults = new(coreTier.VirtualFileSystemEntities);
                    DirectoryViewModel.ChangeSelectedEntity(coreTier);
                    DirectoryViewModel.SetPath(coreTier);
                }

                return Task.CompletedTask;
            });

        public IAsyncCommand<VirtualFileSystemEntity> OpenTreeFolderCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(coreEntity =>
            {
                if (coreEntity is VirtualFileSystemCoreTier coreTier)
                {
                    DirectoryViewModel.ActiveSearchResults = new(coreTier.VirtualFileSystemEntities);
                    DirectoryViewModel.ChangeSelectedEntity(coreTier);
                    // TODO It needs to be addressed to properly set path with Home segment
                    DirectoryViewModel.InitializePath(DirectoryViewModel.VirtualFileSystem?.Home);
                    DirectoryViewModel.SetPath(coreTier);
                }

                return Task.CompletedTask;
            });

        public IAsyncCommand SelectCommand =>
            new AsyncCommand(() =>
            {
                var selected = DirectoryViewModel.VirtualFileSystem.GetCurrentlySelected();
                updatePathAction(TrimUnsopportedSegments(selected.AbsolutePath));

                return Task.CompletedTask;
            });

        private string TrimUnsopportedSegments(string path)
        {
            return path.Replace("Your Files\\", string.Empty).Replace("Home\\", string.Empty);
        }

        public DirectoryViewModel DirectoryViewModel { get; set; }
    }
}