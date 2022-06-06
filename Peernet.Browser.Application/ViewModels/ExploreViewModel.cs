using AsyncAwaitBestPractices.MVVM;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Data;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using System;
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

        private int pageIndex;
        private int pagesCount;
        private int pageSize = 15;
        private int totalResultsCount = 999;

        public ExploreViewModel(IExploreService exploreService, IDownloadManager downloadManager, INavigationService navigationService, IEnumerable<IPlayButtonPlug> playButtonPlugs)
        {
            this.exploreService = exploreService;
            this.downloadManager = downloadManager;
            this.navigationService = navigationService;
            this.playButtonPlugs = playButtonPlugs;

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
            new AsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.QueueUpDownload(downloadModel);
                });

        public IAsyncCommand FirstPageCommand => new AsyncCommand(GoToFirstPage);

        public bool IsLoaded
        {
            get => isLoaded;
            set
            {
                isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        public IAsyncCommand LastPageCommand => new AsyncCommand(GoToLastPage);

        public IAsyncCommand NextPageCommand => new AsyncCommand(GoToNextPage);

        public IAsyncCommand<DownloadModel> OpenCommand =>
                            new AsyncCommand<DownloadModel>(
                model =>
                {
                    var param = new FilePreviewViewModelParameter(model.File, async () => await downloadManager.QueueUpDownload(model), "Download");
                    navigationService.Navigate<FilePreviewViewModel, FilePreviewViewModelParameter>(param);

                    return Task.CompletedTask;
                });

        public int PageIndex
        {
            get => pageIndex;
            set
            {
                pageIndex = value;
                OnPropertyChanged(nameof(PageIndex));
            }
        }

        public int PagesCount
        {
            get => pagesCount;
            set
            {
                pagesCount = value;
                OnPropertyChanged(nameof(PagesCount));
            }
        }

        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value;
                OnPropertyChanged(nameof(PageSize));
            }
        }

        public IAsyncCommand PreviousPageCommand => new AsyncCommand(GoToPreviousPage);

        public IAsyncCommand<DownloadModel> ResumeCommand =>
                                    new AsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.ResumeDownload(downloadModel.Id);
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

                    var results = await exploreService.GetFiles(200, (int)category.Type);
                    SetPlayerState(results);
                    ActiveSearchResults = new ObservableCollection<DownloadModel>(results);
                });

        public IAsyncCommand<DownloadModel> StreamFileCommand =>
            new AsyncCommand<DownloadModel>(
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

        public async Task ReloadResults()
        {
            var exploreResult = await exploreService.GetFiles(200);
            SetPlayerState(exploreResult);
            ActiveSearchResults = new ObservableCollection<DownloadModel>(exploreResult);
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

        private async Task GoToFirstPage() => await GoToPage(0);

        private async Task GoToLastPage() => await GoToPage((int)Math.Ceiling(totalResultsCount / (double)PageSize));

        private async Task GoToNextPage()
        {
            var lastPageIndex = (int)Math.Ceiling(totalResultsCount / (double)PageSize);
            if (PageIndex < lastPageIndex)
            {
                await GoToPage(PageIndex + 1);
            }
        }

        private async Task GoToPage(int pageIndex)
        {
            PageIndex = pageIndex;
            await ReloadResults();
        }

        private async Task GoToPreviousPage()
        {
            if (PageIndex > 0)
            {
                await GoToPage(PageIndex - 1);
            }
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

        private void SetPlayerState(List<DownloadModel> results)
        {
            results.Foreach(r =>
            {
                r.IsPlayerEnabled = playButtonPlugs.Any(plug => plug?.IsSupported(r.File) == true);
            });
        }
    }
}