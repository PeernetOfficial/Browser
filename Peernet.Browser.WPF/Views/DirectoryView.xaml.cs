using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.WPF.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for DictionaryView.xaml
    /// </summary>
    public partial class DirectoryView : UserControl
    {
        public DirectoryView()
        {
            InitializeComponent();
        }

        private void TabControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                var context = (DirectoryViewModel)DataContext;
                var tab = (FrameworkElement)e.OriginalSource;

                if (tab.DataContext is UserDirectoryViewModel userDirectoryViewModel)
                {
                    context.CloseTab(userDirectoryViewModel);
                }
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
                    var homeViewModel = tabControl.DataContext as DirectoryViewModel;
                    if (homeViewModel != null)
                    {
                        var tabs = homeViewModel.DirectoryTabs;
                        var sourceTab = tabItemSource.DataContext as DirectoryTabViewModel;
                        var targetTab = tabItemTarget.DataContext as DirectoryTabViewModel;
                        if (sourceTab != null && sourceTab is not CurrentUserDirectoryViewModel && targetTab is not CurrentUserDirectoryViewModel)
                        {
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