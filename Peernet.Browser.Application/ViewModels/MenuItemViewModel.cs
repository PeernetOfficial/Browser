using AsyncAwaitBestPractices.MVVM;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class MenuItemViewModel : ViewModelBase
    {
        public MenuItemViewModel(string text, Action action = null)
        {
            Text = text;
            Command = new AsyncCommand(() =>
            {
                // navigate to some ViewModel
                action?.Invoke();

                return Task.CompletedTask;
            });
        }

        public IAsyncCommand Command { get; }

        public string Text { get; }
    }
}