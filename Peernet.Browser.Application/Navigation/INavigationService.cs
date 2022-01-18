using Peernet.Browser.Application.ViewModels;
using System;

namespace Peernet.Browser.Application.Navigation
{
    public interface INavigationService
    {
        ViewModelBase CurrentViewModel { get; set; }

        void Navigate<TViewModel>() where TViewModel : ViewModelBase;

        void Navigate<TViewModel, TParameter>(TParameter parameter) where TViewModel : GenericViewModelBase<TParameter> where TParameter : class;

        event Action StateChanged;
    }
}