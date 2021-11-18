﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Presentation.Footer;

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
                 if (result.Status != BlockchainStatus.StatusOK)
                 {
                     GlobalContext.Notifications.Add(new Notification($"Failed to update User. Status: {result.Status}", Severity.Error));
                 }
             }

             UserContext.ReloadContext();

             GlobalContext.IsMainWindowActive = true;

             await mvxNavigationService.Close(this);
         });

        public IUserContext UserContext { get; set; }
    }
}