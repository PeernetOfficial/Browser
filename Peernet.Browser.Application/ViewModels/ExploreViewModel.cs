using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Peernet.Browser.Application.Facades;

namespace Peernet.Browser.Application.ViewModels
{
    public class ExploreViewModel : MvxViewModel
    {
        public ObservableCollection<ApiBlockRecordFile> activeSearchResults;
        private readonly IExploreFacade exploreFacade;
        private readonly IDownloadManager downloadManager;
        private static List<VirtualFileSystemCategory> categoryTypes = GetCategoryTypes();
        private IReadOnlyCollection<ApiBlockRecordFile> sharedFiles;

        public ExploreViewModel(IExploreFacade exploreFacade, IDownloadManager downloadManager)
        {
            this.exploreFacade = exploreFacade;
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
                async category =>
                {
                    if (category.IsSelected)
                    {
                        return;
                    }

                    categoryTypes.ForEach(c => c.ResetSelection());

                    category.IsSelected = true;
                    if (category.Type == VirtualFileSystemEntityType.Binary)
                    {
                        ActiveSearchResults =
                            new ObservableCollection<ApiBlockRecordFile>((await exploreFacade.GetFiles(20, -2)).Files);
                    }
                    else
                    {
                        ActiveSearchResults = new ObservableCollection<ApiBlockRecordFile>((await exploreFacade
                            .GetFiles(20, (int)category.Type)).Files);
                    }
                });

        public override async Task Initialize()
        {
            var exploreResult = await exploreFacade.GetFiles(20);
            sharedFiles = new ReadOnlyCollection<ApiBlockRecordFile>(exploreResult.Files);
            ActiveSearchResults = new ObservableCollection<ApiBlockRecordFile>(sharedFiles);

            await base.Initialize();
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