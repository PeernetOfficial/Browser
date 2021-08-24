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
        
        public HomeViewModel(NavigationBarViewModel navigationBarViewModel, FooterViewModel footerViewModel, ControlButtonsViewModel controlButtonsViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            FooterViewModel = footerViewModel;
            ControlButtonsViewModel = controlButtonsViewModel;
        }


        public NavigationBarViewModel NavigationBarViewModel { get; private set; }
        public FooterViewModel FooterViewModel { get; private set; }
        public ControlButtonsViewModel ControlButtonsViewModel { get; }

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
            this.ControlButtonsViewModel.Prepare();
        }
    }
}
