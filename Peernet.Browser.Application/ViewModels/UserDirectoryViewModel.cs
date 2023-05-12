using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Interfaces;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Domain.Search;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class UserDirectoryViewModel : DirectoryTabViewModel, IShareableContent
    {
        private Func<SearchResult, Task<FileModel>> createResultsSnapshot;
        private SearchResult searchResult;

        public UserDirectoryViewModel(
            string title,
            SearchResult searchResult,
            Func<SearchResult, Task<FileModel>> createResultsSnapshot,
            Func<DirectoryTabViewModel, Task> removeTabAction,
            IVirtualFileSystemFactory virtualFileSystemFactory,
            IEnumerable<IPlayButtonPlug> playButtonPlugs)
            : base(
                  title,
                  virtualFileSystemFactory,
                  playButtonPlugs)
        {
            this.searchResult = searchResult;
            this.createResultsSnapshot = createResultsSnapshot;

            Files = searchResult.Files;
            DeleteCommand = new AsyncCommand(async () => await removeTabAction(this));
            Initialize();
            InitializePath(VirtualFileSystem?.Home);
            OpenCommand.Execute(VirtualFileSystem?.Home);
        }

        public IAsyncCommand DeleteCommand { get; }

        public override bool IsReadOnly => true;

        public Task<FileModel> CreateResultsSnapshot() => createResultsSnapshot(searchResult);

        private void Initialize()
        {
            IsLoaded = false;
            CreateVirtualFileSystem();
            IsLoaded = true;
        }
    }
}