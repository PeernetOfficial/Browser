using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private string searchInput;

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

        public IMvxAsyncCommand Search
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    SearchInput = "Searching...";

                    return Task.CompletedTask;
                });
            }
        }
    }
}
