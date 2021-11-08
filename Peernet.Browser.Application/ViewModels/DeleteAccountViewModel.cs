using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Application.ViewModels
{
    public class DeleteAccountViewModel : MvxViewModel
    {
        private readonly IApplicationManager applicationManager;
        private readonly IAccountService accountService;
        private bool isPolicyAccepted;
        private readonly IUserContext userContext;

        public DeleteAccountViewModel(IApplicationManager applicationManager, IAccountService accountService, IUserContext userContext)
        {
            this.applicationManager = applicationManager;
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

        public IMvxCommand CloseCommand => new MvxCommand(() => applicationManager.CloseModal());

        public IMvxAsyncCommand DeleteAccountCommand => new MvxAsyncCommand(async () =>
        {
            await accountService.Delete(IsPolicyAccepted);
            userContext.ReloadContext();
            applicationManager.CloseModal();
        });
    }
}