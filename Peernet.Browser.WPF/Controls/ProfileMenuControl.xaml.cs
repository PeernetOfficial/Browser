using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.WPF.Extensions;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProfileMenuControl.xaml
    /// </summary>
    public partial class ProfileMenuControl : UserControl
    {
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

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel main)
            {
                main.OpenAboutTab?.Invoke();
            }

            GlobalContext.IsProfileMenuVisible = false;
        }
    }
}