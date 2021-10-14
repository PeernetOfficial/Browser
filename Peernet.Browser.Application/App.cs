using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.Application
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<HomeViewModel>();
        }

        protected override IMvxViewModelLocator CreateDefaultViewModelLocator()
        {
            return new SingletonViewModelLocator();
        }
    }
}