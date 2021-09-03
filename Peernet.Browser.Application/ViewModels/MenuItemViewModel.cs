using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.ViewModels
{
    public class MenuItemViewModel : MvxViewModel
    {
        public MenuItemViewModel(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
