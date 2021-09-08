using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using System;
using System.Threading.Tasks;

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

        public IMvxAsyncCommand UploadFileCommand => new MvxAsyncCommand(() => 
        {
            return Task.CompletedTask;
        });

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(() =>
        {
            UserContext.ReloadContext();

            GlobalContext.IsMainWindowActive = true;
            return mvxNavigationService.Close(this);
        });

        public IMvxAsyncCommand SaveChangesCommand => new MvxAsyncCommand(() =>
        {
            // Add check for value change in relation to previous
            if (UserContext.HasUserChanged)
            {
                profileService.AddUserName(UserContext.User.Name);
                profileService.AddUserImage(UserContext.User.Image);
            }

            UserContext.ReloadContext();

            GlobalContext.IsMainWindowActive = true;
            return mvxNavigationService.Close(this);
        });

        public IUserContext UserContext { get; set; }
    }
}