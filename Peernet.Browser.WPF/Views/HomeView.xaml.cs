using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Peernet.Browser.WPF.Extensions;

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
            if (string.IsNullOrEmpty(viewModel.SearchInput))
            {
                return;
            }

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

        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var tabItemTarget = e.OriginalSource.GetVisualTreeParent<TabItem>();
            if (tabItemTarget != null)
            {
                var tabItemSource = (TabItem)e.Data.GetData(typeof(TabItem));
                if (tabItemTarget != tabItemSource)
                {
                    var homeViewModel = tabControl.DataContext as HomeViewModel;
                    if (homeViewModel != null)
                    {
                        var tabs = homeViewModel.Tabs;
                        var sourceTab = tabItemSource.DataContext as SearchTabElementViewModel;
                        if (sourceTab != null)
                        {
                            var targetTab = tabItemTarget.DataContext as SearchTabElementViewModel;
                            int targetIndex = tabs.IndexOf(targetTab);
                            tabs.Remove(sourceTab);
                            tabs.Insert(targetIndex, sourceTab);
                            homeViewModel.SelectedIndex = targetIndex;
                        }
                    }
                }
            }
        }

        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var tabItem = e.Source as TabItem;

            if (tabItem != null && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
            }
        }
    }
}