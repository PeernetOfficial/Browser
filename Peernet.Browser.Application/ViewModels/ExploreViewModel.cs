using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.ViewModels
{
    public class ExploreViewModel : MvxViewModel
    {
        public ObservableCollection<DownloadModel> activeSearchResults;
        private readonly IExploreService exploreService;
        private readonly IDownloadManager downloadManager;
        private static readonly List<VirtualFileSystemCategory> categoryTypes = GetCategoryTypes();
        private IReadOnlyCollection<DownloadModel> sharedFiles;

        public ExploreViewModel(IExploreService exploreService, IDownloadManager downloadManager)
        {
            this.exploreService = exploreService;
            this.downloadManager = downloadManager;
        }

        public ObservableCollection<DownloadModel> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public ObservableCollection<VirtualFileSystemCategory> CategoryTypes => new(categoryTypes);

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
                            new ObservableCollection<DownloadModel>((await exploreService.GetFiles(20, -2)));
                    }
                    else
                    {
                        ActiveSearchResults = new ObservableCollection<DownloadModel>(await exploreService
                            .GetFiles(20, (int)category.Type));
                    }
                });

        public override async Task Initialize()
        {
            var exploreResult = await exploreService.GetFiles(20);
            sharedFiles = new ReadOnlyCollection<DownloadModel>(exploreResult);
            ActiveSearchResults = new ObservableCollection<DownloadModel>(sharedFiles);

            await base.Initialize();
        }

        private static VirtualFileSystemCategory GetCategory(VirtualFileSystemEntityType type)
        {
            return new VirtualFileSystemCategory(type.ToString(), type, null);
        }

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