using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.ViewModels
{
    public class AboutViewModel : MvxViewModel
    {
        public AboutViewModel(IMvxNavigationService navigationService)
        {
            BackCommand = new MvxCommand(() => navigationService.Close(this));
        }

        public IMvxCommand BackCommand { get; }
    }
}