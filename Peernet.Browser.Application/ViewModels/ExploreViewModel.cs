using AsyncAwaitBestPractices.MVVM;
using DevExpress.Mvvm.Xpf;
using DevExpress.Utils.Serializing;
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
        public List<DownloadModel> cachedSearchResults;
        public bool isLoaded;
        private static readonly List<VirtualFileSystemCoreCategory> categoryTypes = GetCategoryTypes();
        private readonly IDownloadManager downloadManager;
        private readonly IExploreService exploreService;
        private readonly INavigationService navigationService;
        private readonly IEnumerable<IPlayButtonPlug> playButtonPlugs;

        private int pageIndex;
        private int pageSize = 15;
        private int totalResultsCount = 200;
        private const int minimumResultsLimit = 5;

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

        public List<DownloadModel> CachedSearchResults
        {
            get => cachedSearchResults;
            set
            {
                cachedSearchResults = value;
                OnPropertyChanged(nameof(CachedSearchResults));
                OnPropertyChanged(nameof(PagesCount));
            }
        }

        public ObservableCollection<VirtualFileSystemCoreCategory> CategoryTypes => new(categoryTypes);

        public IAsyncCommand<DownloadModel> DownloadCommand =>
            new AsyncCommand<DownloadModel>(
                async downloadModel =>
                {
                    await downloadManager.QueueUpDownload(downloadModel);
                });

        public IAsyncCommand FirstPageCommand => new AsyncCommand(() =>
        {
            GoToFirstPage();
            return Task.CompletedTask;
        });

        public bool IsLoaded
        {
            get => isLoaded;
            set
            {
                isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        public IAsyncCommand LastPageCommand => new AsyncCommand(() =>
        {
            GoToLastPage();
            return Task.CompletedTask;
        });

        public IAsyncCommand NextPageCommand => new AsyncCommand(() =>
        {
            GoToNextPage();
            return Task.CompletedTask;
        });

        public IAsyncCommand<DownloadModel> OpenCommand => new AsyncCommand<DownloadModel>(
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
                if (value > PagesCount)
                {
                    pageIndex = PagesCount;
                }
                else if (value <= 0)
                {
                    pageIndex = 1;
                }
                else
                {
                    pageIndex = value;
                }

                ReloadActiveResultsFromCache();
                OnPropertyChanged(nameof(PageIndex));
            }
        }

        public int PagesCount
        {
            get
            {
                if (!CachedSearchResults.IsNullOrEmpty())
                {
                    return (int)Math.Ceiling(CachedSearchResults.Count / (double)PageSize);
                }

                return 1;
            }
        }

        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value;
                GoToPage(1);
                OnPropertyChanged(nameof(PageSize));
                OnPropertyChanged(nameof(PagesCount));
            }
        }

        public IAsyncCommand PreviousPageCommand => new AsyncCommand(() =>
        {
            GoToPreviousPage();
            return Task.CompletedTask;
        });

        public IAsyncCommand<DownloadModel> ResumeCommand => new AsyncCommand<DownloadModel>(
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
                    
                    await ReloadResults();
                    ReloadActiveResultsFromCache();
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

        public int TotalResultsCount
        {
            get => totalResultsCount;
            set
            {
                if (value > minimumResultsLimit)
                {
                    totalResultsCount = value;
                }
                else
                {
                    totalResultsCount = minimumResultsLimit;
                }
                OnPropertyChanged(nameof(TotalResultsCount));
                ReloadResults();
            }
        }

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
            var category = categoryTypes.FirstOrDefault(ct => ct.IsSelected);
            var type = category?.Type;
            var exploreResult = await exploreService.GetFiles(TotalResultsCount, (int?)type);
            SetPlayerState(exploreResult);
            CachedSearchResults = exploreResult;
            GoToFirstPage();
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

        private void GoToFirstPage() => GoToPage(1);

        private void GoToLastPage() => GoToPage(PagesCount);

        private void GoToNextPage()
        {
            if (PageIndex < PagesCount)
            {
                GoToPage(PageIndex + 1);
            }
        }

        private void GoToPage(int pageIndex)
        {
            PageIndex = pageIndex;
        }

        private void GoToPreviousPage()
        {
            if (PageIndex > 1)
            {
                GoToPage(PageIndex - 1);
            }
        }

        private async Task LoadResults()
        {
            var results = new List<DownloadModel>();

            for (int i = 0; i < 7 && results.IsNullOrEmpty(); i++)
            {
                results = await exploreService.GetFiles(TotalResultsCount);
                Thread.Sleep(7000);
            }

            SetPlayerState(results);

            IsLoaded = true;
            CachedSearchResults = results;
            GoToPage(1);
        }

        public void ReloadActiveResultsFromCache()
        {
            if (CachedSearchResults != null)
            {
                var startingIndex = (PageIndex - 1) * PageSize;
                int length;
                if ((startingIndex + PageSize) > CachedSearchResults.Count)
                {
                    length = CachedSearchResults.Count - startingIndex;
                }
                else
                {
                    length = PageSize;
                }

                ActiveSearchResults = new ObservableCollection<DownloadModel>(CachedSearchResults.GetRange(startingIndex, length));
            }
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