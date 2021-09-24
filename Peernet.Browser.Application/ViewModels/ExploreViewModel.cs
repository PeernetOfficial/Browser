using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;

namespace Peernet.Browser.Application.ViewModels
{
    public class ExploreViewModel : MvxViewModel
    {
        public List<ApiBlockRecordFile> activeSearchResults;
        private readonly IExploreService exploreService;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private IReadOnlyCollection<ApiBlockRecordFile> sharedFiles;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;

        public ExploreViewModel(IVirtualFileSystemFactory virtualFileSystemFactory, IExploreService exploreService)
        {
            this.virtualFileSystemFactory = virtualFileSystemFactory;
            this.exploreService = exploreService;
        }

        public IMvxAsyncCommand<ApiBlockRecordFile> DownloadCommand =>
            new MvxAsyncCommand<ApiBlockRecordFile>(
                apiBlockRecordFile =>
                {
                    // Asynchronous download operation to be performed

                    return Task.CompletedTask;
                });

        public List<ApiBlockRecordFile> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public override Task Initialize()
        {
            var exploreResult = exploreService.GetFiles(50);
            sharedFiles = new ReadOnlyCollection<ApiBlockRecordFile>(exploreResult.Files);
            ActiveSearchResults = sharedFiles.ToList();

            return base.Initialize();
        }
    }
}