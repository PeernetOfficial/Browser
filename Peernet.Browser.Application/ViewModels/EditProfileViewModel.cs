using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        private readonly IModalNavigationService mvxNavigationService;
        private readonly IProfileService profileService;
        private readonly INotificationsManager notificationsManager;

        public EditProfileViewModel(IModalNavigationService mvxNavigationService, IUserContext userContext, IProfileService profileService, INotificationsManager notificationsManager)
        {
            this.mvxNavigationService = mvxNavigationService;
            this.profileService = profileService;
            this.notificationsManager = notificationsManager;

            UserContext = userContext;
        }

        public IAsyncCommand CloseCommand => new AsyncCommand(() =>
         {
             UserContext.ReloadContext();

             GlobalContext.IsMainWindowActive = true;
             mvxNavigationService.Close();
             return Task.CompletedTask;
         });

        public IAsyncCommand RemovePhotoCommand => new AsyncCommand(() =>
         {
             mvxNavigationService.Navigate<DeleteAccountViewModel>();
             return Task.CompletedTask;
         });

        public IAsyncCommand SaveChangesCommand => new AsyncCommand(async () =>
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
                     notificationsManager.Notifications.Add(new Notification(message, details, Severity.Error));
                 }
             }

             UserContext.ReloadContext();

             GlobalContext.IsMainWindowActive = true;

             mvxNavigationService.Close();
         });

        public IUserContext UserContext { get; set; }
    }
}