using MvvmCross.ViewModels;

namespace Peernet.Browser.Application
{
    public class App : MvxApplication
    {
        protected override IMvxViewModelLocator CreateDefaultViewModelLocator()
        {
            return new SingletonViewModelLocator();
        }
    }
}