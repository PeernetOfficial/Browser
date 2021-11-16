using System.Windows;
using System.Windows.Controls;
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
            if (((DependencyObject)e.OriginalSource).FindParent<ProfileMenuControl>() == null)
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