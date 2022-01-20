using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Threading.Tasks;
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
                var viewModel = (HomeViewModel)DataContext;
                var terminalViewModel = App.ServiceProvider.GetRequiredService<TerminalViewModel>();
                Task.Run(() => terminalViewModel.Prepare(new()).ConfigureAwait(false).GetAwaiter().GetResult());

                if (viewModel.SearchInput.Equals("debug", StringComparison.InvariantCultureIgnoreCase))
                {
                    var terminal = new TerminalWindow(terminalViewModel);
                    terminal.Show();
                    viewModel.SearchInput = string.Empty;
                }
                else
                {
                    await viewModel.SearchCommand?.ExecuteAsync();
                }
            }
        }
    }
}