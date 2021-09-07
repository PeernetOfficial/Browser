using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditProfileViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;

        public EditProfileViewModel(IMvxNavigationService mvxNavigationService, IUserContext userContext)
        {
            this.mvxNavigationService = mvxNavigationService;
            UserContext = userContext;
        }

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(() =>
        {
            GlobalContext.IsMainWindowActive = true;
            return mvxNavigationService.Close(this);
        });

        public IUserContext UserContext { get; set; }
    }
}