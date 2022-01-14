using Peernet.Browser.Application.ViewModels;
using System;

namespace Peernet.Browser.Application.Navigation
{
    public interface INavigationService
    {
        ViewModelBase CurrentViewModel { get; set; }

        void Navigate<TViewModel>() where TViewModel : ViewModelBase;

        event Action StateChanged;
    }
}