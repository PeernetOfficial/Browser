using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.SDK.Models.Presentation.Footer;
using System.Net.Http;

namespace Peernet.Browser.Application.ViewModels
{
    public class DeleteAccountViewModel : ViewModelBase
    {
        private readonly IModalNavigationService navigationService;
        private readonly IAccountService accountService;
        private bool isPolicyAccepted;
        private readonly IUserContext userContext;
        private readonly INotificationsManager notificationsManager;

        public DeleteAccountViewModel(IModalNavigationService navigationService, IAccountService accountService, IUserContext userContext, INotificationsManager notificationsManager)
        {
            this.navigationService = navigationService;
            this.accountService = accountService;
            this.userContext = userContext;
            this.notificationsManager = notificationsManager;
        }

        public bool IsPolicyAccepted
        {
            get => isPolicyAccepted;
            set
            {
                isPolicyAccepted = value;
                OnPropertyChanged(nameof(IsPolicyAccepted));
            }
        }

        public IAsyncCommand CloseCommand => new AsyncCommand(() =>
         {
             navigationService.Close();
             return System.Threading.Tasks.Task.CompletedTask;
         });

        public IAsyncCommand DeleteAccountCommand => new AsyncCommand(async () =>
          {
              try
              {
                  await accountService.Delete(IsPolicyAccepted);
              }
              catch (HttpRequestException ex)
              {
                  var message = $"Failed to delete account. Status: {ex.Message}";
                  notificationsManager.Notifications.Add(new Notification(message,
                      MessagingHelper.GetApiSummary($"{nameof(accountService)}.{nameof(accountService.Delete)}"),
                      Severity.Error));
              }

              userContext.ReloadContext();
              navigationService.Close();
          });
    }
}