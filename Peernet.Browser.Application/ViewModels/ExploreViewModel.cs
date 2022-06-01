using AsyncAwaitBestPractices.MVVM;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Data;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using INavigationService = Peernet.Browser.Application.Navigation.INavigationService;

namespace Peernet.Browser.Application.ViewModels
{
    public class ExploreViewModel : ViewModelBase
    {
        public ObservableCollection<DownloadModel> activeSearchResults;
        public bool isLoaded;
        private static readonly List<VirtualFileSystemCoreCategory> categoryTypes = GetCategoryTypes();
        private readonly IDownloadManager downloadManager;
        private readonly IExploreService exploreService;
        private readonly INavigationService navigationService;
        private readonly IEnumerable<IPlayButtonPlug> playButtonPlugs;

        public ExploreViewModel(IExploreService exploreService, IDownloadManager downloadManager, INavigationService navigationService, IEnumerable<IPlayButtonPlug> playButtonPlugs)
        {
            this.exploreService = exploreService;
            this.downloadManager = downloadManager;
            this.navigationService = navigationService;
            this.playButtonPlugs = playButtonPlugs;

            FetchDataCommand = new DelegateCommand<FetchPageAsyncArgs>(FetchData);
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

        public ObservableCollection<VirtualFileSystemCoreCategory> CategoryTypes => new(categoryTypes);

        public IAsyncCommand<DownloadModel> DownloadCommand =>
            new AsyncAwaitBestPractices.MVVM.AsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.QueueUpDownload(downloadModel);
                });

        public DelegateCommand<FetchPageAsyncArgs> FetchDataCommand { get; private set; }

        public bool IsLoaded
        {
            get => isLoaded;
            set
            {
                isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        public IAsyncCommand<DownloadModel> OpenCommand =>
            new AsyncAwaitBestPractices.MVVM.AsyncCommand<DownloadModel>(
                model =>
                {
                    var param = new FilePreviewViewModelParameter(model.File, async () => await downloadManager.QueueUpDownload(model), "Download");
                    navigationService.Navigate<FilePreviewViewModel, FilePreviewViewModelParameter>(param);

                    return Task.CompletedTask;
                });

        public IAsyncCommand<DownloadModel> ResumeCommand =>
            new AsyncAwaitBestPractices.MVVM.AsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.ResumeDownload(downloadModel.Id);
                });

        public IAsyncCommand<VirtualFileSystemCoreCategory> SelectCategoryCommand =>
            new AsyncAwaitBestPractices.MVVM.AsyncCommand<VirtualFileSystemCoreCategory>(
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

                    var results = await exploreService.GetFiles(200, (int)category.Type);
                    SetPlayerState(results);
                    ActiveSearchResults = new ObservableCollection<DownloadModel>(results);
                });

        public IAsyncCommand<DownloadModel> StreamFileCommand =>
            new AsyncAwaitBestPractices.MVVM.AsyncCommand<DownloadModel>(
                model =>
                {
                    playButtonPlugs.Foreach(async plug =>
                    {
                        if (plug?.IsSupported(model.File) == true)
                        {
                            await plug?.Execute(model.File);
                        }
                    });

                    return Task.CompletedTask;
                });

        public void FetchData(FetchPageAsyncArgs args)
        {
            args.Result = GetResults(args);
        }

        public async Task<FetchRowsResult> GetResults(FetchPageAsyncArgs args)
        {
            var files = (await exploreService.GetPagedFiles(args.Skip / args.Take, args.Take)).ToArray();
            return new FetchRowsResult(files, true);
        }

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

        private async Task LoadResults()
        {
            var results = new List<DownloadModel>();

            for (int i = 0; i < 7 && results.IsNullOrEmpty(); i++)
            {
                results = await exploreService.GetFiles(200);
                Thread.Sleep(7000);
            }

            SetPlayerState(results);

            IsLoaded = true;
            ActiveSearchResults = new ObservableCollection<DownloadModel>(results);
        }

        private async Task ReloadResults()
        {
            var exploreResult = await exploreService.GetFiles(200);
            SetPlayerState(exploreResult);
            ActiveSearchResults = new ObservableCollection<DownloadModel>(exploreResult);
        }

        private void SetPlayerState(List<DownloadModel> results)
        {
            results.Foreach(r =>
            {
                r.IsPlayerEnabled = playButtonPlugs.Any(plug => plug?.IsSupported(r.File) == true);
            });
        }
        private bool hasMorePages = true;
        private int pageSize = 15;
        private int pageIndex;
        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value;
                OnPropertyChanged(nameof(PageSize));
            }
        }

        public int PageIndex
        {
            get => pageIndex;
            set
            {
                pageIndex = value;
                OnPropertyChanged(nameof(PageIndex));
            }
        }

        public bool HasMorePages
        {
            get => hasMorePages;
            set
            {
                hasMorePages = value;
                OnPropertyChanged(nameof(HasMorePages));
            }
        }
    }
}