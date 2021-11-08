using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditProfileViewModel : MvxViewModel
    {
        private readonly IApplicationManager applicationManager;
        private readonly IProfileService profileService;

        public EditProfileViewModel(IApplicationManager applicationManager, IUserContext userContext, IProfileService profileService)
        {
            this.applicationManager = applicationManager;
            this.profileService = profileService;

            UserContext = userContext;
        }

        public IMvxCommand CloseCommand => new MvxCommand(() =>
        {
            UserContext.ReloadContext();
            applicationManager.CloseModal();
        });

        public IMvxCommand RemovePhotoCommand => new MvxCommand(() =>
        {
            applicationManager.NavigateToMain(ViewType.DeleteAccount);
        });

        public IMvxAsyncCommand SaveChangesCommand => new MvxAsyncCommand(async () =>
        {
            if (UserContext.HasUserChanged)
            {
                await profileService.UpdateUser(UserContext.User.Name, UserContext.User.Image);
            }

            UserContext.ReloadContext();
            applicationManager.CloseModal();
        });

        public IUserContext UserContext { get; set; }
    }
}