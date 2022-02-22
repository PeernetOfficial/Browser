using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly ISearchService searchService;
        private readonly IDownloadManager downloadManager;
        private readonly INavigationService navigationService;
        private readonly IPlayButtonPlug playButtonPlug;
        private string searchInput;
        private int selectedIndex = -1;

        public HomeViewModel(ISearchService searchService, IDownloadManager downloadManager, INavigationService navigationService, IPlayButtonPlug playButtonPlug)
        {
            this.searchService = searchService;
            this.downloadManager = downloadManager;
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this.playButtonPlug = playButtonPlug;

            SearchCommand = new AsyncCommand(Search);
            Tabs.CollectionChanged += (o, s) =>
            {
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(IsNotVisible));
                OnPropertyChanged(nameof(Alignment));
                GlobalContext.IsLogoVisible = IsVisible;
            };

            GlobalContext.IsLogoVisible = IsVisible;
        }

        public Alignments Alignment => IsVisible ? Alignments.Stretch : Alignments.Center;

        public bool IsNotVisible => !IsVisible;

        public bool IsVisible => Tabs.Any();

        public IAsyncCommand SearchCommand { get; }

        public string SearchInput
        {
            get => searchInput;
            set
            {
                searchInput = value;
                OnPropertyChanged(nameof(SearchInput));
            }
        }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(Content));
            }
        }

        public SearchTabElementViewModel Content => SelectedIndex < 0 ? null : Tabs[SelectedIndex];

        public ObservableCollection<SearchTabElementViewModel> Tabs { get; } = new ObservableCollection<SearchTabElementViewModel>();

        private async Task RemoveTab(SearchTabElementViewModel e)
        {
            await searchService.Terminate(e.Filters.UuId);
            Tabs.Remove(e);
            SelectedIndex = IsVisible ? 0 : -1;
        }

        private async Task DownloadFile(SearchResultRowModel row)
        {
            await downloadManager.QueueUpDownload(new DownloadModel(row.File));
        }

        private void OpenFile(SearchResultRowModel row)
        {
            var param = new FilePreviewViewModelParameter(row.File, false,
                async () => await downloadManager.QueueUpDownload(new DownloadModel(row.File)), "Download");
            navigationService.Navigate<FilePreviewViewModel, FilePreviewViewModelParameter>(param);
        }

        private void ExecutePlayButtonPlug(SearchResultRowModel model) => playButtonPlug?.Execute(model.File);

        private Task Search()
        {
            var toAdd = new SearchTabElementViewModel(SearchInput, RemoveTab, searchService.Search, DownloadFile, OpenFile, ExecutePlayButtonPlug);
            Tabs.Add(toAdd);
            SelectedIndex = Tabs.Count - 1;
            SearchInput = string.Empty;

            return Task.CompletedTask;
        }
    }
}