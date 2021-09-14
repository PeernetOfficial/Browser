using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel, ISearchable
    {
        private bool _showSearchBox = false;
        private string searchInput;
        private bool showHint;

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

        public string SearchInput
        {
            get => searchInput;
            set
            {
                searchInput = value;
                RaisePropertyChanged(nameof(SearchInput));
            }
        }
        public bool ShowHint
        {
            get => showHint;
            set { SetProperty(ref showHint, value); }
        }
        public bool ShowSearchBox
        {
            get => _showSearchBox;
            set { SetProperty(ref _showSearchBox, value); }
        }
    }
}