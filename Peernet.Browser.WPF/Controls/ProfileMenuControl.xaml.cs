using System.Windows;
using System.Windows.Controls;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProfileMenuControl.xaml
    /// </summary>
    public partial class ProfileMenuControl : UserControl
    {
        public ProfileMenuControl() => InitializeComponent();

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