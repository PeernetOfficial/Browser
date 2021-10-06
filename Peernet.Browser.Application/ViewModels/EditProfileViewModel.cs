using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Facades;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditProfileViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IProfileFacade profileFacade;

        public EditProfileViewModel(IMvxNavigationService mvxNavigationService, IUserContext userContext, IProfileFacade profileFacade)
        {
            this.mvxNavigationService = mvxNavigationService;
            this.profileFacade = profileFacade;

            UserContext = userContext;
        }

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(() =>
        {
            UserContext.ReloadContext();

            GlobalContext.IsMainWindowActive = true;
            return mvxNavigationService.Close(this);
        });

        public IMvxAsyncCommand RemovePhotoCommand => new MvxAsyncCommand(() =>
        {
            profileFacade.DeleteUserImage();

            return Task.CompletedTask;
        });

        public IMvxAsyncCommand SaveChangesCommand => new MvxAsyncCommand(() =>
        {
            if (UserContext.HasUserChanged)
            {
                profileFacade.UpdateUser(UserContext.User.Name, UserContext.User.Image);
            }

            UserContext.ReloadContext();

            GlobalContext.IsMainWindowActive = true;
            return mvxNavigationService.Close(this);
        });

        public IUserContext UserContext { get; set; }
    }
}