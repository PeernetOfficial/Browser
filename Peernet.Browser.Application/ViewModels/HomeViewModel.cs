using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using System.Linq;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private string searchInput;
        private int selectedIndex = -1;

        public HomeViewModel()
        {
            SearchCommand = new MvxCommand(Search);
            Tabs.CollectionChanged += (o, s) =>
            {
                RaisePropertyChanged(nameof(IsVisible));
                RaisePropertyChanged(nameof(IsNotVisible));
                RaisePropertyChanged(nameof(Alignment));
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

        public SearchContentElement Content => SelectedIndex < 0 ? null : Tabs[SelectedIndex].Content;

        public MvxObservableCollection<SearchTabElement> Tabs { get; } = new MvxObservableCollection<SearchTabElement>();

        private void RemoveTab(SearchTabElement e)
        {
            Tabs.Remove(e);
            SelectedIndex = IsVisible ? 0 : -1;
        }

        private void Search()
        {
            var toAdd = new SearchTabElement(SearchInput, RemoveTab);
            Tabs.Add(toAdd);
            SearchInput = "";
            SelectedIndex = Tabs.Count - 1;
        }
    }
}