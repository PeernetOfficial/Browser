using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;

namespace Peernet.Browser.Application.ViewModels
{
    public class ExploreViewModel : MvxViewModel
    {
        public ObservableCollection<ApiBlockRecordFile> activeSearchResults;
        private readonly IExploreService exploreService;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private IReadOnlyCollection<ApiBlockRecordFile> sharedFiles;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;
        private List<VirtualFileSystemEntityType> categoryTypes;

        public List<VirtualFileSystemEntityType> CategoryTypes => categoryTypes;

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
        
        public IMvxAsyncCommand<VirtualFileSystemEntityType> SelectCategoryCommand =>
            new MvxAsyncCommand<VirtualFileSystemEntityType>(
                type =>
                {
                    ActiveSearchResults = new ObservableCollection<ApiBlockRecordFile>(exploreService.GetFiles(50, LowLevelFileType.Document).Files);

                    return Task.CompletedTask;
                });


        public ObservableCollection<ApiBlockRecordFile> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public override Task Initialize()
        {
            var exploreResult = exploreService.GetFiles(50);
            sharedFiles = new ReadOnlyCollection<ApiBlockRecordFile>(exploreResult.Files);
            ActiveSearchResults = new ObservableCollection<ApiBlockRecordFile>(sharedFiles);

            categoryTypes = GetCategoryTypes();

            return base.Initialize();
        }

        private static List<VirtualFileSystemEntityType> GetCategoryTypes()
        {
            return new List<VirtualFileSystemEntityType>
            {
                VirtualFileSystemEntityType.Document,
                VirtualFileSystemEntityType.Video,
                VirtualFileSystemEntityType.Audio,
                VirtualFileSystemEntityType.Ebook,
                VirtualFileSystemEntityType.Picture,
                VirtualFileSystemEntityType.Text,
                VirtualFileSystemEntityType.Binary
            };
        }
    }
}