using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.WPF.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProfileMenuControl.xaml
    /// </summary>
    public partial class ProfileMenuControl : UserControl
    {
        private WidgetsWindow widgetsWindow;

        public ProfileMenuControl()
        {
            InitializeComponent();
            App.MainWindowClicked += OnMainWindowClicked;
        }

        private void OnMainWindowClicked(object sender, RoutedEventArgs e)
        {
            var dependencyObject = (DependencyObject)e.OriginalSource;
            FrameworkElement templatedParentFrameworkElement = null;
            if (VisualTreeHelper.GetChildrenCount(dependencyObject) > 0)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, 0);
                var childFrameworkElement = child as FrameworkElement;
                templatedParentFrameworkElement = childFrameworkElement?.TemplatedParent as FrameworkElement;
            }

            if (dependencyObject.FindParent<ProfileMenuControl>() == null &&
                templatedParentFrameworkElement?.Name != "AccountPopupToggle")
            {
                GlobalContext.IsProfileMenuVisible = false;
            }
        }

        private void About_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel main)
            {
                main.OpenAboutTab?.Invoke();
            }

            GlobalContext.IsProfileMenuVisible = false;
        }

        private void WidgetsSettings_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (widgetsWindow == null)
            {
                widgetsWindow = new WidgetsWindow(DataContext as MainViewModel);
                widgetsWindow.Closing += (sender, e) =>
                {
                    e.Cancel = true;
                    widgetsWindow.Visibility = Visibility.Hidden;
                };
            }

            widgetsWindow.Show();
        }
    }
}