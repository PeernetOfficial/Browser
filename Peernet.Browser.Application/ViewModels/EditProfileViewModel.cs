using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Profile;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        private readonly IModalNavigationService modalNavigationService;
        private readonly IProfileService profileService;
        private readonly INotificationsManager notificationsManager;
        private readonly IUserContext userContext;

        public EditProfileViewModel(IModalNavigationService mvxNavigationService, IUserContext userContext, IProfileService profileService, INotificationsManager notificationsManager)
        {
            this.modalNavigationService = mvxNavigationService;
            this.profileService = profileService;
            this.notificationsManager = notificationsManager;

            this.userContext = userContext;
            User = userContext.User.DeepCopy();
        }

        public IAsyncCommand CloseCommand => new AsyncCommand(() =>
           {
               userContext.ReloadContext();
               User = userContext.User.DeepCopy();
               modalNavigationService.Close();

               return Task.CompletedTask;
           });

        public IAsyncCommand RemovePhotoCommand => new AsyncCommand(() =>
           {
               modalNavigationService.Navigate<DeleteAccountViewModel>();
               return Task.CompletedTask;
           });
        
        public IAsyncCommand SaveChangesCommand => new AsyncCommand(async () =>
           {
               if (userContext.User != User)
               {
                   var result = await profileService.UpdateUser(User.Name, User.Image);
                   if (result is not { Status: BlockchainStatus.StatusOK })
                   {
                       var message = $"Failed to update User. Status: {result?.Status.ToString() ?? "[Unknown]"}";
                       var details =
                           MessagingHelper.GetApiSummary(
                               $"{nameof(profileService)}.{nameof(profileService.UpdateUser)}") +
                           MessagingHelper.GetInOutSummary(User, result);
                       notificationsManager.Notifications.Add(new Notification(message, details, Severity.Error));
                   }
                   
                   userContext.ReloadContext();
               }
               
               modalNavigationService.Close();
           });

        public User User { get; set; }
    }
}