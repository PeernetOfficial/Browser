using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Application.ViewModels
{
    public class DeleteAccountViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IProfileService profileService;

        public DeleteAccountViewModel(IMvxNavigationService mvxNavigationService, IProfileService profileService)
        {
            this.mvxNavigationService = mvxNavigationService;
            this.profileService = profileService;
        }

        public IMvxCommand CloseCommand => new MvxCommand(() => mvxNavigationService.Close(this));

        public IMvxAsyncCommand DeleteAccountCommand => new MvxAsyncCommand(async () => await profileService.DeleteUserImage());
    }
}