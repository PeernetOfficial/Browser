using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using System.Threading.Tasks;
using Peernet.Browser.Application.Services;


namespace Peernet.Browser.Application.ViewModels
{
    public class EditProfileViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IProfileService profileService;

        public EditProfileViewModel(IMvxNavigationService mvxNavigationService, IUserContext userContext, IProfileService profileService)
        {
            this.mvxNavigationService = mvxNavigationService;
            this.profileService = profileService;

            UserContext = userContext;
        }

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () =>
        {
            UserContext.ReloadContext();

            GlobalContext.IsMainWindowActive = true;
            await mvxNavigationService.Close(this);
        });

        public IMvxAsyncCommand RemovePhotoCommand => new MvxAsyncCommand(async () =>
        {
            await mvxNavigationService.Navigate<DeleteAccountViewModel>();
        });

        public IMvxAsyncCommand SaveChangesCommand => new MvxAsyncCommand(async () =>
        {
            if (UserContext.HasUserChanged)
            {
                await profileService.UpdateUser(UserContext.User.Name, UserContext.User.Image);
            }

            UserContext.ReloadContext();

            GlobalContext.IsMainWindowActive = true;
            
            await mvxNavigationService.Close(this);
        });

        public IUserContext UserContext { get; set; }
    }
}