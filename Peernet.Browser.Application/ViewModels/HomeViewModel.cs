using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private string searchInput;
        private bool showHint;

        public string SearchInput
        {
            get => searchInput;
            set
            {
                searchInput = value;
                RaisePropertyChanged(nameof(SearchInput));
            }
        }

        public HomeViewModel()
        {
        }

        public bool ShowHint
        {
            get => showHint;
            set { SetProperty(ref showHint, value); }
        }

        private bool _showSearchBox = false;

        public bool ShowSearchBox
        {
            get => _showSearchBox;
            set { SetProperty(ref _showSearchBox, value); }
        }

        public IMvxCommand Search
        {
            get
            {
                return new MvxCommand(() =>
                {
                    SearchInput = "Searching...";
                });
            }
        }

        public IMvxCommand RemoveHint
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (ShowHint)
                    {
                        ShowHint = false;
                        ShowSearchBox = true;
                    }
                });
            }
        }
    }
}