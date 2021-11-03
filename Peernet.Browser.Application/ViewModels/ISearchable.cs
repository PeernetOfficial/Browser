using MvvmCross.Commands;

namespace Peernet.Browser.Application.ViewModels
{
    public interface ISearchable
    {
        IMvxCommand SearchCommand { get; }

        IMvxCommand RemoveHint { get; }

        public string SearchInput { get; set; }

        public bool ShowHint { get; set; }

        public bool ShowSearchBox { get; set; }
    }
}