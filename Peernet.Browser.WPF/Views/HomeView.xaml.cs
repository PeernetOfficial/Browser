using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.ViewModels;
using System;
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
                await terminalViewModel.Prepare(new());

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