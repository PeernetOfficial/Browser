using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel, ISearchable
    {
        private string searchInput;
        private bool showHint = true;
        private bool showSearchBox = false;

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
            set => SetProperty(ref searchInput, value);
        }

        public bool ShowHint
        {
            get => showHint;
            set => SetProperty(ref showHint, value);
        }

        public bool ShowSearchBox
        {
            get => showSearchBox;
            set => SetProperty(ref showSearchBox, value);
        }
    }
}