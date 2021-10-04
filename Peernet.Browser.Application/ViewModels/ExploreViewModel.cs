using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Peernet.Browser.Application.Download;

namespace Peernet.Browser.Application.ViewModels
{
    public class ExploreViewModel : MvxViewModel
    {
        public ObservableCollection<ApiBlockRecordFile> activeSearchResults;
        private readonly IExploreService exploreService;
        private readonly IDownloadManager downloadManager;
        private List<VirtualFileSystemCategory> categoryTypes;
        private IReadOnlyCollection<ApiBlockRecordFile> sharedFiles;

        public ExploreViewModel(IExploreService exploreService, IDownloadManager downloadManager)
        {
            this.exploreService = exploreService;
            this.downloadManager = downloadManager;
        }

        public ObservableCollection<ApiBlockRecordFile> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public ObservableCollection<VirtualFileSystemCategory> CategoryTypes => new(categoryTypes);

        public IMvxAsyncCommand<ApiBlockRecordFile> DownloadCommand =>
            new MvxAsyncCommand<ApiBlockRecordFile>(
                apiBlockRecordFile =>
                {
                    downloadManager.QueueUpDownload(apiBlockRecordFile);

                    return Task.CompletedTask;
                });

        public IMvxAsyncCommand<VirtualFileSystemCategory> SelectCategoryCommand =>
            new MvxAsyncCommand<VirtualFileSystemCategory>(
                category =>
                {
                    if (category.IsSelected)
                    {
                        return Task.CompletedTask;
                    }

                    categoryTypes.ForEach(c => c.ResetSelection());

                    category.IsSelected = true;
                    if (category.Type == VirtualFileSystemEntityType.Binary)
                    {
                        ActiveSearchResults =
                            new ObservableCollection<ApiBlockRecordFile>(exploreService.GetFiles(20, -2).Files);
                    }
                    else
                    {
                        ActiveSearchResults = new ObservableCollection<ApiBlockRecordFile>(exploreService
                            .GetFiles(20, (int)category.Type).Files);
                    }

                    return Task.CompletedTask;
                });

        public override Task Initialize()
        {
            var exploreResult = exploreService.GetFiles(20);
            sharedFiles = new ReadOnlyCollection<ApiBlockRecordFile>(exploreResult.Files);
            ActiveSearchResults = new ObservableCollection<ApiBlockRecordFile>(sharedFiles);

            categoryTypes = GetCategoryTypes();

            return base.Initialize();
        }

        private static VirtualFileSystemCategory GetCategory(VirtualFileSystemEntityType type)
        {
            return new VirtualFileSystemCategory(type.ToString(), type, null);
        }

        // It could return mapping Tuple where Keys are Categories and Values are integer values representing Type. (Binary, -2);(Document,5)
        private static List<VirtualFileSystemCategory> GetCategoryTypes()
        {
            return new List<VirtualFileSystemCategory>
            {
                GetCategory(VirtualFileSystemEntityType.Document),
                GetCategory(VirtualFileSystemEntityType.Video),
                GetCategory(VirtualFileSystemEntityType.Audio),
                GetCategory(VirtualFileSystemEntityType.Ebook),
                GetCategory(VirtualFileSystemEntityType.Picture),
                GetCategory(VirtualFileSystemEntityType.Text),
                GetCategory(VirtualFileSystemEntityType.Binary)
            };
        }
    }
}