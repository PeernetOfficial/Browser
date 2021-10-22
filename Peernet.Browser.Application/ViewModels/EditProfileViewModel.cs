﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using System.Threading.Tasks;
using Peernet.Browser.Application.Services;

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

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(() =>
        {
            UserContext.ReloadContext();

            GlobalContext.IsMainWindowActive = true;
            return mvxNavigationService.Close(this);
        });

        public IMvxAsyncCommand RemovePhotoCommand => new MvxAsyncCommand(() =>
        {
            mvxNavigationService.Navigate<DeleteAccountViewModel>();

            return Task.CompletedTask;
        });

        public IMvxAsyncCommand SaveChangesCommand => new MvxAsyncCommand(() =>
        {
            if (UserContext.HasUserChanged)
            {
                profileService.UpdateUser(UserContext.User.Name, UserContext.User.Image);
            }

            UserContext.ReloadContext();

            GlobalContext.IsMainWindowActive = true;
            return mvxNavigationService.Close(this);
        });

        public IUserContext UserContext { get; set; }
    }
}