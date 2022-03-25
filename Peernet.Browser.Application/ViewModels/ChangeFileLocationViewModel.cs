using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class ChangeFileLocationViewModel : ViewModelBase
    {
        private FileModel model;

        public ChangeFileLocationViewModel(DirectoryViewModel directoryViewModel, FileModel model)
        {
            DirectoryViewModel = directoryViewModel;
            this.model = model;
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

        public DirectoryViewModel DirectoryViewModel { get; set; }

        public IAsyncCommand<VirtualFileSystemEntity> OpenTreeFolderCommand =>
            new AsyncCommand<VirtualFileSystemEntity>(coreEntity =>
            {
                if (coreEntity is VirtualFileSystemCoreTier coreTier)
                {
                    DirectoryViewModel.ActiveSearchResults = new(coreTier.VirtualFileSystemEntities);
                    DirectoryViewModel.ChangeSelectedEntity(coreTier);

                    DirectoryViewModel.InitializePath(DirectoryViewModel.VirtualFileSystem?.Home);
                    DirectoryViewModel.SetPath(DirectoryViewModel.VirtualFileSystem?.Home);
                    DirectoryViewModel.SetPath(coreTier);
                }

                return Task.CompletedTask;
            });

        public IAsyncCommand SelectCommand =>
            new AsyncCommand(() =>
            {
                var selected = DirectoryViewModel.VirtualFileSystem.GetCurrentlySelected();
                model.Directory = TrimUnsopportedSegments(selected.AbsolutePath);

                return Task.CompletedTask;
            });

        private string TrimUnsopportedSegments(string path)
        {
            return path.Replace("Your Files\\", string.Empty).Replace("Home\\", string.Empty);
        }
    }
}