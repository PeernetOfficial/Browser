using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class MenuItemViewModel : MvxViewModel
    {
        public MenuItemViewModel(string text, Action action = null)
        {
            Text = text;
            Command = new MvxAsyncCommand(() =>
            {
                // navigate to some ViewModel
                if (action != null)
                {
                    action();
                }

                return Task.CompletedTask;
            });
        }

        public IMvxAsyncCommand Command { get; }
     
        public string Text { get; }
    }
}