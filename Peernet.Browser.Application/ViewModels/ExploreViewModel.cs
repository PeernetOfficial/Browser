using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.VirtualFileSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Models.Presentation.Footer;
using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Navigation;
using System.Threading;
using System.Linq;

namespace Peernet.Browser.Application.ViewModels
{
    public class ExploreViewModel : ViewModelBase
    {
        public ObservableCollection<DownloadModel> activeSearchResults;
        public bool isLoaded;
        private readonly IExploreService exploreService;
        private readonly IDownloadManager downloadManager;
        private readonly INavigationService navigationService;
        private static readonly List<VirtualFileSystemCoreCategory> categoryTypes = GetCategoryTypes();

        public ExploreViewModel(IExploreService exploreService, IDownloadManager downloadManager, INavigationService navigationService)
        {
            this.exploreService = exploreService;
            this.downloadManager = downloadManager;
            this.navigationService = navigationService;

            Task.Run(() => LoadResults().ConfigureAwait(false).GetAwaiter().GetResult());
        }

        public ObservableCollection<DownloadModel> ActiveSearchResults
        {
            get => activeSearchResults;
            set
            {
                activeSearchResults = value;
                OnPropertyChanged(nameof(ActiveSearchResults));
            }
        }

        public bool IsLoaded
        {
            get => isLoaded;
            set
            {
                isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        public ObservableCollection<VirtualFileSystemCoreCategory> CategoryTypes => new(categoryTypes);

        public IAsyncCommand<DownloadModel> DownloadCommand =>
            new AsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.QueueUpDownload(downloadModel);
                });

        public IAsyncCommand<DownloadModel> ResumeCommand =>
            new AsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.ResumeDownload(downloadModel.Id);
                });

        public IAsyncCommand<DownloadModel> OpenCommand =>
            new AsyncCommand<DownloadModel>(
                model =>
                {
                    var param = new FilePreviewViewModelParameter(model.File, false, async () => await downloadManager.QueueUpDownload(model), "Download");
                    navigationService.Navigate<FilePreviewViewModel, FilePreviewViewModelParameter>(param);

                    return Task.CompletedTask;
                });

        public IAsyncCommand<VirtualFileSystemCoreCategory> SelectCategoryCommand =>
            new AsyncCommand<VirtualFileSystemCoreCategory>(
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
                            .GetFiles(200, (int)category.Type));
                });

        private static VirtualFileSystemCoreCategory GetCategory(VirtualFileSystemEntityType type)
        {
            return new VirtualFileSystemCoreCategory(type.ToString(), type, new List<VirtualFileSystemEntity>());
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
            var exploreResult = await exploreService.GetFiles(200);
            ActiveSearchResults = new ObservableCollection<DownloadModel>(exploreResult);
        }

        private async Task LoadResults()
        {
            var results = new List<DownloadModel>();

            for (int i = 0; i < 7 && results.IsNullOrEmpty(); i++)
            {
                results = await exploreService.GetFiles(200);
                Thread.Sleep(7000);
            }

            IsLoaded = true;
            ActiveSearchResults = new ObservableCollection<DownloadModel>(results);
        }
    }
}