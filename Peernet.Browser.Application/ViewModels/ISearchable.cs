using AsyncAwaitBestPractices.MVVM;

namespace Peernet.Browser.Application.ViewModels
{
    public interface ISearchable
    {
        IAsyncCommand SearchCommand { get; }

        IAsyncCommand RemoveHint { get; }

        public string SearchInput { get; set; }

        public bool ShowHint { get; set; }

        public bool ShowSearchBox { get; set; }
    }
}