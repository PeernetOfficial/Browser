using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Navigation
{
    public class ModalNavigationService : IModalNavigationService
    {
        private readonly IServiceProvider serviceProvider;

        private ViewModelBase currentViewModel;

        public ModalNavigationService(IServiceProvider serviceProvider)
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

        public bool IsOpen => CurrentViewModel != null;

        public void Close()
        {
            CurrentViewModel = null;
        }

        public Task Navigate<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = serviceProvider.GetRequiredService<TViewModel>();
            CurrentViewModel = viewModel;

            return Task.CompletedTask;
        }

        public async Task Navigate<TViewModel, TParameter>(TParameter parameter) where TViewModel : GenericViewModelBase<TParameter> where TParameter : class
        {
            var viewModel = serviceProvider.GetRequiredService<TViewModel>();
            await viewModel.Prepare(parameter);
            CurrentViewModel = viewModel;
        }
    }
}