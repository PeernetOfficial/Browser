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
        
        public HomeViewModel(NavigationBarViewModel navigationBarViewModel, FooterViewModel footerViewModel, IApplicationManager applicationManager)
        {
            this.NavigationBarViewModel = navigationBarViewModel;
            this.FooterViewModel = footerViewModel;
            this.applicationManager = applicationManager;
        }


        public NavigationBarViewModel NavigationBarViewModel { get; private set; }
        public FooterViewModel FooterViewModel { get; private set; }

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

            this.FooterViewModel.Prepare();
            this.NavigationBarViewModel.Prepare();
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
