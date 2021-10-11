using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Managers;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class ButtonsViewModel : MvxViewModel
    {
        private readonly IApplicationManager applicationManager;

        public ButtonsViewModel(IApplicationManager applicationManager)
        {
            this.applicationManager = applicationManager;
        }

        public IMvxAsyncCommand CloseAppCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    CloseApp();

                    return Task.CompletedTask;
                });
            }
        }

        public IMvxAsyncCommand MaximizeCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    Maximize();

                    return Task.CompletedTask;
                });
            }
        }

        public IMvxAsyncCommand MinimizeCommand
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    Minimize();

                    return Task.CompletedTask;
                });
            }
        }

        private void CloseApp()
        {
            this.applicationManager.Shutdown();
        }

        private void Maximize()
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

        private void Minimize()
        {
            this.applicationManager.Minimize();
        }
    }
}