using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation;
using System.Linq;
using Peernet.Browser.Application.Facades;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly ISearchFacade searchFacade;
        private string searchInput;
        private int selectedIndex = -1;

        public HomeViewModel(ISearchFacade searchFacade)
        {
            this.searchFacade = searchFacade;
            SearchCommand = new MvxCommand(Search);
            Tabs.CollectionChanged += (o, s) =>
            {
                RaisePropertyChanged(nameof(IsVisible));
                RaisePropertyChanged(nameof(IsNotVisible));
                RaisePropertyChanged(nameof(Alignment));
                GlobalContext.IsLogoVisible = IsVisible;
            };
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

        public SearchContentElementViewModel Content => SelectedIndex < 0 ? null : Tabs[SelectedIndex].Content;

        public MvxObservableCollection<SearchTabElementViewModel> Tabs { get; } = new MvxObservableCollection<SearchTabElementViewModel>();

        private void RemoveTab(SearchTabElementViewModel e)
        {
            Tabs.Remove(e);
            SelectedIndex = IsVisible ? 0 : -1;
        }

        private void Search()
        {
            // todo: It should be considered to make it separate view. Then this SearchTabElementViewModel should inherit from MvxViewModel and here should be just Navigation using mvxnavigationservice. SearchFacade should not be dependency of HomeViewModel but SearchTabElementViewModel and should be injected there.
            var toAdd = new SearchTabElementViewModel(SearchInput, RemoveTab, searchFacade);
            Tabs.Add(toAdd);
            SearchInput = "";
            SelectedIndex = Tabs.Count - 1;
        }
    }
}