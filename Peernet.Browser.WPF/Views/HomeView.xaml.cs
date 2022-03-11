using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView() => InitializeComponent();

        private async void SearchText_Entered(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await RunSearch();
            }
        }

        private async void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await RunSearch();
        }

        private async Task RunSearch()
        {
            var viewModel = (HomeViewModel)DataContext;
            if (viewModel.SearchInput.Equals("debug", StringComparison.InvariantCultureIgnoreCase))
            {
                var terminalViewModel = App.ServiceProvider.GetRequiredService<TerminalViewModel>();
                var source = new CancellationTokenSource();

                // TODO: This part of the code has few issues: 1. The call is not awaited, because of that returns AggregateException and specific exception can't be really handled; 2. Cancellation of the request is not done gently.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => terminalViewModel.Prepare(new(source)).ConfigureAwait(false).GetAwaiter().GetResult(), source.Token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                var terminal = new TerminalWindow(terminalViewModel);
                terminal.Show();
                viewModel.SearchInput = string.Empty;
            }
            else
            {
                await viewModel.SearchCommand?.ExecuteAsync();
            }
        }

        private async void TabControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                var context = (HomeViewModel)DataContext;
                var tab = (FrameworkElement)e.OriginalSource;
                var tabContext = (SearchTabElementViewModel)tab.DataContext;
                await context.RemoveTab(tabContext);
            }
        }
    }
}