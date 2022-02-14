using Peernet.Browser.Application.ViewModels;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Navigation
{
    public interface INavigationService
    {
        ViewModelBase CurrentViewModel { get; set; }

        Task Navigate<TViewModel>() where TViewModel : ViewModelBase;

        Task Navigate<TViewModel, TParameter>(TParameter parameter) where TViewModel : GenericViewModelBase<TParameter> where TParameter : class;

        event Action StateChanged;
    }
}