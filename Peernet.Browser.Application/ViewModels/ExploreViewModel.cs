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
        public ObservableCollection<ApiBlockRecordFile> activeSearchResults;
        private readonly IExploreService exploreService;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private IReadOnlyCollection<ApiBlockRecordFile> sharedFiles;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;
        private List<VirtualFileSystemCategory> categoryTypes;

        public ObservableCollection<VirtualFileSystemCategory> CategoryTypes => new(categoryTypes);

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

        public IMvxAsyncCommand<VirtualFileSystemCategory> SelectCategoryCommand =>
            new MvxAsyncCommand<VirtualFileSystemCategory>(
                category =>
                {
                    categoryTypes.ForEach(c => c.ResetSelection());

                    category.IsSelected = true;
                    if (category.Type == VirtualFileSystemEntityType.Binary)
                    {
                        ActiveSearchResults =
                            new ObservableCollection<ApiBlockRecordFile>(exploreService.GetFiles(20, -2).Files);
                    }
                    //if (type == VirtualFileSystemEntityType.Ebook)
                    //{
                    //    ActiveSearchResults =
                    //        new ObservableCollection<ApiBlockRecordFile>(exploreService.GetFiles(50, ?).Files);
                    //}
                    else
                    {
                        ActiveSearchResults = new ObservableCollection<ApiBlockRecordFile>(exploreService
                            .GetFiles(20, (int)category.Type).Files);
                    }

                    return Task.CompletedTask;
                });


        public ObservableCollection<ApiBlockRecordFile> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public override Task Initialize()
        {
            var exploreResult = exploreService.GetFiles(20);
            sharedFiles = new ReadOnlyCollection<ApiBlockRecordFile>(exploreResult.Files);
            ActiveSearchResults = new ObservableCollection<ApiBlockRecordFile>(sharedFiles);

            categoryTypes = GetCategoryTypes();

            return base.Initialize();
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

        private static VirtualFileSystemCategory GetCategory(VirtualFileSystemEntityType type)
        {
            return new VirtualFileSystemCategory(type.ToString(), type, null);
        }
    }
}