using MvvmCross.Commands;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public readonly INotifyChange<string> SearchInput = new NotifyChange<string>();
        
        public HomeViewModel()
        {
        }

        public IMvxAsyncCommand Search
        {
            get
            {
                return new MvxAsyncCommand(() =>
                {
                    SearchInput.Value = "Searching...";

                    return Task.CompletedTask;
                });
            }
        }
    }
}
