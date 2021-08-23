using MvvmCross.Commands;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public readonly INotifyChange<string> SearchInput = new NotifyChange<string>();
        
        private readonly IApplicationManager applicationManager;
        private readonly NavigationBarViewModel navigationBarViewModel;
        private readonly FooterViewModel footerViewModel;

        public HomeViewModel(NavigationBarViewModel navigationBarViewModel, FooterViewModel footerViewModel, IApplicationManager applicationManager)
        {
            this.navigationBarViewModel = navigationBarViewModel;
            this.footerViewModel = footerViewModel;
            this.applicationManager = applicationManager;
        }

        public IMvxAsyncCommand Search
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    SearchInput.Value = "Searching...";

                    return Task.CompletedTask;
                });
            }
        }

        public override void Prepare()
        {
            base.Prepare();

            this.footerViewModel.Prepare();
            this.navigationBarViewModel.Prepare();
        }

        // application control methods
        public void CloseApp()
        {
            this.applicationManager.Shutdown();
        }

        public void Maximize()
        {
            if (this.applicationManager.IsMaximized)
            {
                this.applicationManager.Restore();
            } 
            else 
            { 
                this.applicationManager.Maximize(); 
            }
        }

        public void Minimize()
        {
            this.applicationManager.Minimize();
        }

    }
}
