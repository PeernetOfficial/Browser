using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class MenuItemViewModel : MvxViewModel
    {
        public MenuItemViewModel(string text)
        {
            Text = text;

            // todo: it should come from constructor
            Command = new MvxAsyncCommand(() =>
            {
                // navigate here to some ViewModel

                return Task.CompletedTask;
            });
        }

        public string Text { get; }

        public IMvxAsyncCommand Command { get; }
    }
}
