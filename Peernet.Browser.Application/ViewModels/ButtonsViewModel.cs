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


    }
}