using MvvmCross.Commands;

namespace Peernet.Browser.Application.ViewModels
{
    public interface ISearchable
    {
        IMvxCommand Search { get; }

        IMvxCommand RemoveHint { get; }

        public string SearchInput { get; set; }

        public bool ShowHint { get; set; }

        public bool ShowSearchBox { get; set; }
    }
}