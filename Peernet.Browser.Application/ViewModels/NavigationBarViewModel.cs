using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;

namespace Peernet.Browser.Application.ViewModels
{
    public class NavigationBarViewModel : MvxViewModel
    {
        public NavigationBarViewModel(IApplicationManager applicationManager, IUserContext userContext)
        {
            UserContext = userContext;

            NavigateExploreCommand = new MvxCommand(() => applicationManager.NavigateToMain(ViewType.Explorer));
            NavigateHomeCommand = new MvxCommand(() => applicationManager.NavigateToMain(ViewType.Home, false));
            NavigateDirectoryCommand = new MvxCommand(() => applicationManager.NavigateToMain(ViewType.Directory));
            EditProfileCommand = new MvxCommand(() => applicationManager.NavigateToModal(ViewType.EditProfile));
            NavigateAboutCommand = new MvxCommand(() => applicationManager.NavigateToMain(ViewType.About));
            OpenCloseProfileMenuCommand = new MvxCommand(() => { GlobalContext.IsProfileMenuVisible ^= true; });
        }

        public IMvxCommand NavigateDirectoryCommand { get; }

        public IMvxCommand NavigateExploreCommand { get; }

        public IMvxCommand NavigateHomeCommand { get; }

        public IMvxCommand EditProfileCommand { get; }

        public IMvxCommand NavigateAboutCommand { get; }

        public IMvxCommand OpenCloseProfileMenuCommand { get; }

        public IUserContext UserContext { get; set; }
    }
}