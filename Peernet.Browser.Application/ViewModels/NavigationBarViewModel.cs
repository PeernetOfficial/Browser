using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Navigation;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {

        public NavigationBarViewModel(IModalNavigationService modalNavigationService, IUserContext userContext)
        {
            UserContext = userContext;

            EditProfileCommand = new AsyncCommand(() =>
            {
                modalNavigationService.Navigate<EditProfileViewModel>();

                return Task.CompletedTask;
            });

            OpenCloseProfileMenuCommand = new AsyncCommand(() =>
            {
                GlobalContext.IsProfileMenuVisible ^= true;
                return Task.CompletedTask;
            });
        }

        public IAsyncCommand EditProfileCommand { get; }

        public IAsyncCommand OpenCloseProfileMenuCommand { get; }

        public IUserContext UserContext { get; set; }
    }
}