using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private string searchInput;

        public HomeViewModel()
        {
            Search = new MvxCommand(SearchAction);
        }

        private void SearchAction()
        {
            SearchInput = "";
        }

        public IMvxCommand Search { get; }

        public string SearchInput
        {
            get => searchInput;
            set => SetProperty(ref searchInput, value);
        }
    }
}