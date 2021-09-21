using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public SearchModeModel Model { get; } = new();

        public HomeViewModel()
        {
            Search = new MvxCommand(Model.Clear);
            Tabs.Add(new SearchTabElement { Title = "Snowden documentary", Content = "Snowden documentary" });
            Tabs.Add(new SearchTabElement { Title = "Micro hydro power", Content = "Micro hydro power" });
            Tabs.Add(new SearchTabElement { Title = "File sharing", Content = "File sharing" });
        }

        public IMvxCommand Search { get; }

        public MvxObservableCollection<SearchTabElement> Tabs { get; } = new MvxObservableCollection<SearchTabElement>();
    }

    public class SearchTabElement : MvxNotifyPropertyChanged
    {
        private string title;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string content;

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }
    }

    public class SearchModeModel : MvxNotifyPropertyChanged
    {
        private Alignments alignment = Alignments.Center;

        public Alignments Alignment
        {
            get => alignment;
            set => SetProperty(ref alignment, value);
        }

        private string searchInput;

        public string SearchInput
        {
            get => searchInput;
            set => SetProperty(ref searchInput, value);
        }

        private bool imgVisibility = true;

        public bool ImgVisibility
        {
            get => imgVisibility;
            set
            {
                SetProperty(ref imgVisibility, value);
                RaisePropertyChanged(nameof(ImgVisibilityRev));
            }
        }

        public bool ImgVisibilityRev => !ImgVisibility;

        public void Clear()
        {
            SearchInput = "";
            ChangeMode();
        }

        private bool isSearched;

        private void ChangeMode()
        {
            isSearched = !isSearched;
            ImgVisibility = !isSearched;
            Alignment = isSearched ? Alignments.Stretch : Alignments.Center;
        }
    }
}