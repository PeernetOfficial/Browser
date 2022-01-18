using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Managers;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class ButtonsViewModel : ViewModelBase
    {
        private readonly IApplicationManager applicationManager;

        public ButtonsViewModel(IApplicationManager applicationManager)
        {
            this.applicationManager = applicationManager;
        }

        public IAsyncCommand CloseAppCommand
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    CloseApp();

                    return Task.CompletedTask;
                });
            }
        }

        public IAsyncCommand MaximizeCommand
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    Maximize();

                    return Task.CompletedTask;
                });
            }
        }

        public IAsyncCommand MinimizeCommand
        {
            get
            {
                return new AsyncCommand(() =>
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