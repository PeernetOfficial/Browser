using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Footer;
using System.Net.Http;
using Peernet.Browser.Application.Utilities;

namespace Peernet.Browser.Application.ViewModels
{
    public class DeleteAccountViewModel : ViewModelBase
    {
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IAccountService accountService;
        private bool isPolicyAccepted;
        private readonly IUserContext userContext;

        public DeleteAccountViewModel(IMvxNavigationService mvxNavigationService, IAccountService accountService, IUserContext userContext)
        {
            this.mvxNavigationService = mvxNavigationService;
            this.accountService = accountService;
            this.userContext = userContext;
        }

        public override void ViewDisappeared()
        {
            IsPolicyAccepted = false;
            base.ViewDisappeared();
        }

        public bool IsPolicyAccepted
        {
            get => isPolicyAccepted;
            set => SetProperty(ref isPolicyAccepted, value);
        }

        public IMvxCommand CloseCommand => new MvxCommand(() => mvxNavigationService.Close(this));

        public IAsyncCommand DeleteAccountCommand => new MvxAsyncCommand(async () =>
         {
             try
             {
                 await accountService.Delete(IsPolicyAccepted);
             }
             catch (HttpRequestException ex)
             {
                 var message = $"Failed to delete account. Status: {ex.Message}";
                 GlobalContext.Notifications.Add(new Notification(message,
                     MessagingHelper.GetApiSummary($"{nameof(accountService)}.{nameof(accountService.Delete)}"),
                     Severity.Error));
             }

             userContext.ReloadContext();
             await mvxNavigationService.Close(this);
         });
    }
}