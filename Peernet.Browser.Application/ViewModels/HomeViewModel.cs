using MvvmCross.Commands;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public readonly INotifyChange<string> SearchInput = new NotifyChange<string>();

        public HomeViewModel(NavigationBarViewModel navigationBarViewModel, FooterViewModel footerViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            FooterViewModel = footerViewModel;
        }

        public NavigationBarViewModel NavigationBarViewModel { get; }

        public FooterViewModel FooterViewModel { get; }

        public IMvxAsyncCommand Search
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    SearchInput.Value = "Searching...";

                    return Task.CompletedTask;
                });
            }
        }

        public override void Prepare()
        {
            base.Prepare();

            FooterViewModel.Prepare();
            NavigationBarViewModel.Prepare();
        }
    }
}
