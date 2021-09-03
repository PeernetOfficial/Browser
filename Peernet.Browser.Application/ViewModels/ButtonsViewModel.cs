using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Application.ViewModels
{
    public class ButtonsViewModel : MvxViewModel
    {
        private readonly IApplicationManager applicationManager;

        public ButtonsViewModel(IApplicationManager applicationManager)
        {
            this.applicationManager = applicationManager;
        }

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