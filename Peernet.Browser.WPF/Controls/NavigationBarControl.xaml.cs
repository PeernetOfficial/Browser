using System.Windows.Input;
using MvvmCross.Plugin.Control.Platforms.Wpf;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for NavigationBarControl.xaml
    /// </summary>
    public partial class NavigationBarControl : MvxWpfControl
    {
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
    }
}