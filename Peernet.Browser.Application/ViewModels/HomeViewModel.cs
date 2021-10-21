using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Footer;
using Peernet.Browser.Models.Presentation.Home;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly ISearchService searchService;
        private readonly IDownloadManager downloadManager;
        private string searchInput;
        private int selectedIndex = -1;

        public HomeViewModel(ISearchService searchService, IDownloadManager downloadManager)
        {
            this.searchService = searchService;
            this.downloadManager = downloadManager;
            SearchCommand = new MvxCommand(Search);
            Tabs.CollectionChanged += (o, s) =>
            {
                RaisePropertyChanged(nameof(IsVisible));
                RaisePropertyChanged(nameof(IsNotVisible));
                RaisePropertyChanged(nameof(Alignment));
                GlobalContext.IsLogoVisible = IsVisible;
            };
        }

        public override void ViewAppeared()
        {
            GlobalContext.IsLogoVisible = IsVisible;
        }

        public Alignments Alignment => IsVisible ? Alignments.Stretch : Alignments.Center;

        public bool IsNotVisible => !IsVisible;
        public bool IsVisible => Tabs.Any();
        public IMvxCommand SearchCommand { get; }

        public string SearchInput
        {
            get => searchInput;
            set => SetProperty(ref searchInput, value);
        }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                SetProperty(ref selectedIndex, value);
                RaisePropertyChanged(nameof(Content));
            }
        }

        public SearchTabElementViewModel Content => SelectedIndex < 0 ? null : Tabs[SelectedIndex];

        public MvxObservableCollection<SearchTabElementViewModel> Tabs { get; } = new MvxObservableCollection<SearchTabElementViewModel>();

        private async Task RemoveTab(SearchTabElementViewModel e)
        {
            await searchService.Terminate(e.Filters.UuId);
            Tabs.Remove(e);
            SelectedIndex = IsVisible ? 0 : -1;
        }

        private async Task DownloadFile(SearchResultRowModel row)
        {
            await downloadManager.QueueUpDownload(new DownloadModel(row.Source));
        }

        private void Search()
        {
            var toAdd = new SearchTabElementViewModel(SearchInput, RemoveTab, searchService.Search, DownloadFile);
            Tabs.Add(toAdd);
            SearchInput = "";
            SelectedIndex = Tabs.Count - 1;
        }
    }
}