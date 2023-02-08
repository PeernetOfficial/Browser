using Peernet.Browser.Application.ViewModels;
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

                if (tab.DataContext is UserDirectoryViewModel userTab)
                {
                    context.CloseTab(userTab);
                }
            }
        }
    }
}