using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider serviceProvider;

        private ViewModelBase currentViewModel;

        public NavigationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public event Action StateChanged;

        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel?.Dispose();

                currentViewModel = value;
                StateChanged?.Invoke();
            }
        }

        public async Task Navigate<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = serviceProvider.GetRequiredService<TViewModel>();
            CurrentViewModel = viewModel;
        }

        public async Task Navigate<TViewModel, TParameter>(TParameter parameter) where TViewModel : GenericViewModelBase<TParameter> where TParameter : class
        {
            var viewModel = serviceProvider.GetRequiredService<TViewModel>();
            await viewModel.Prepare(parameter);
            CurrentViewModel = viewModel;
        }
    }
}