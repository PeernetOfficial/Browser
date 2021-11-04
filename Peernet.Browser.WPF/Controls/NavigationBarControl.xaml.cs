using System.Windows;
using System.Windows.Input;
using MvvmCross.Plugin.Control.Platforms.Wpf;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for NavigationBarControl.xaml
    /// </summary>
    public partial class NavigationBarControl : MvxWpfControl
    {
        public static readonly DependencyProperty IsDarkModeToggledProperty =
            DependencyProperty.Register("IsDarkModeToggled", typeof(bool),
                typeof(NavigationBarControl),
                new FrameworkPropertyMetadata(DarkMode_OnToggle) { BindsTwoWayByDefault = true });

        public bool IsDarkModeToggled
        {
            get => (bool)GetValue(IsDarkModeToggledProperty);
            set => SetValue(IsDarkModeToggledProperty, value);
        }

        private bool GotClicked;

        public NavigationBarControl() => InitializeComponent();

        private void ProfileButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ProfileMenu.IsOpen = true;
        }

        private void ProfileButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!ProfileMenu.IsMouseOver && Mouse.DirectlyOver != null)
            {
                ProfileMenu.IsOpen = false;
            }
        }

        private void ProfileMenu_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (GotClicked && Mouse.DirectlyOver == null)
            {
                GotClicked = false;
                return;
            }

            if (!ProfileButton.IsMouseOver)
            {
                ProfileMenu.IsOpen = false;
            }

            GotClicked = false;
        }

        private void ProfileMenu_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            GotClicked = true;
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