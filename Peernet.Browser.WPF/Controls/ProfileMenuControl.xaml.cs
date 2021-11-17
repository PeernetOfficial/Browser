using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation;
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

        public static readonly DependencyProperty IsDarkModeToggledProperty =
            DependencyProperty.Register("IsDarkModeToggled", typeof(bool),
                typeof(ProfileMenuControl),
                new FrameworkPropertyMetadata(DarkMode_OnToggle) { BindsTwoWayByDefault = true });

        public bool IsDarkModeToggled
        {
            get =>(bool)GetValue(IsDarkModeToggledProperty);
            set => SetValue(IsDarkModeToggledProperty, value);
        }

        private static void DarkMode_OnToggle(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (GlobalContext.VisualMode == VisualMode.LightMode)
            {
                GlobalContext.VisualMode = VisualMode.DarkMode;
            }
            else if (GlobalContext.VisualMode == VisualMode.DarkMode)
            {
                GlobalContext.VisualMode = VisualMode.LightMode;
            }

            (App.Current as App).UpdateAllResources();
        }
    }
}