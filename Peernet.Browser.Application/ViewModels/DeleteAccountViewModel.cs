using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Footer;
using System.Net.Http;

namespace Peernet.Browser.Application.ViewModels
{
    public class DeleteAccountViewModel : MvxViewModel
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

        public IMvxAsyncCommand DeleteAccountCommand => new MvxAsyncCommand(async () =>
         {
             try
             {
                 await accountService.Delete(IsPolicyAccepted);
             }
             catch (HttpRequestException ex)
             {
                 GlobalContext.Notifications.Add(new Notification($"Failed to delete account. Status: {ex.Message}", Severity.Error));
             }

             userContext.ReloadContext();
             await mvxNavigationService.Close(this);
         });
    }
}