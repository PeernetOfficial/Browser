using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
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
                 var result = await profileService.UpdateUser(UserContext.User.Name, UserContext.User.Image);
                 if (result is not { Status: BlockchainStatus.StatusOK })
                 {
                     var message = $"Failed to update User. Status: {result?.Status.ToString() ?? "[Unknown]"}";
                     var details =
                         MessagingHelper.GetApiSummary(
                             $"{nameof(profileService)}.{nameof(profileService.UpdateUser)}") +
                         MessagingHelper.GetInOutSummary(UserContext.User, result);
                     GlobalContext.Notifications.Add(new Notification(message, details, Severity.Error));
                 }
             }

             UserContext.ReloadContext();

             GlobalContext.IsMainWindowActive = true;

             await mvxNavigationService.Close(this);
         });

        public IUserContext UserContext { get; set; }
    }
}