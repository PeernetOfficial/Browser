using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.VirtualFileSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.ViewModels
{
    public class ExploreViewModel : MvxViewModel
    {
        public ObservableCollection<DownloadModel> activeSearchResults;
        private readonly IExploreService exploreService;
        private readonly IDownloadManager downloadManager;
        private readonly IMvxNavigationService navigationService;
        private static readonly List<VirtualFileSystemCoreCategory> categoryTypes = GetCategoryTypes();
        private IReadOnlyCollection<DownloadModel> sharedFiles;

        public ExploreViewModel(IExploreService exploreService, IDownloadManager downloadManager, IMvxNavigationService navigationService)
        {
            this.exploreService = exploreService;
            this.downloadManager = downloadManager;
            this.navigationService = navigationService;
        }

        public ObservableCollection<DownloadModel> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public ObservableCollection<VirtualFileSystemCoreCategory> CategoryTypes => new(categoryTypes);

        public IMvxAsyncCommand<DownloadModel> DownloadCommand =>
            new MvxAsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.QueueUpDownload(downloadModel);
                });

        public IMvxAsyncCommand<DownloadModel> ResumeCommand =>
            new MvxAsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.ResumeDownload(downloadModel.Id);
                });

        public IMvxCommand<DownloadModel> OpenCommand =>
            new MvxCommand<DownloadModel>(
                model =>
                {
                    var param = new FilePreviewViewModelParameter(model.File, false, true, "Download");
                    navigationService.Navigate<FilePreviewViewModel, FilePreviewViewModelParameter>(param);
                });

        public IMvxAsyncCommand<VirtualFileSystemCoreCategory> SelectCategoryCommand =>
            new MvxAsyncCommand<VirtualFileSystemCoreCategory>(
                async category =>
                {
                    if (category.IsSelected)
                    {
                        await ReloadResults();
                        category.IsSelected = false;
                        return;
                    }

                    categoryTypes.ForEach(c => c.ResetSelection());

                    category.IsSelected = true;
                    ActiveSearchResults = new ObservableCollection<DownloadModel>(await exploreService
                            .GetFiles(20, (int)category.Type));
                });

        public override async Task Initialize()
        {
            await ReloadResults();
            await base.Initialize();
        }

        private static VirtualFileSystemCoreCategory GetCategory(VirtualFileSystemEntityType type)
        {
            return new VirtualFileSystemCoreCategory(type.ToString(), type, null);
        }

        private static List<VirtualFileSystemCoreCategory> GetCategoryTypes()
        {
            return new List<VirtualFileSystemCoreCategory>
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

        private async Task ReloadResults()
        {
            var exploreResult = await exploreService.GetFiles(20);
            sharedFiles = new ReadOnlyCollection<DownloadModel>(exploreResult);
            ActiveSearchResults = new ObservableCollection<DownloadModel>(sharedFiles);
        }
    }
}