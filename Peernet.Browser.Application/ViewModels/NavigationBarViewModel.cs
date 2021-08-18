using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public NavigationBarViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public IMvxAsyncCommand NavigateHomeCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    await _navigationService.Navigate<HomeViewModel>();
                });
            }
        }

        public IMvxAsyncCommand NavigateUserCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    return Task.CompletedTask;
                });
            }
        }

        public IMvxAsyncCommand NavigateDictionaryCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    return Task.CompletedTask;
                });
            }
        }

        public IMvxAsyncCommand NavigateSettingsCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    return Task.CompletedTask;
                });
            }
        }
    }
}
