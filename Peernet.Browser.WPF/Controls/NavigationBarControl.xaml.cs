using System.Windows.Input;
using MvvmCross.Plugin.Control.Platforms.Wpf;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for NavigationBarControl.xaml
    /// </summary>
    public partial class NavigationBarControl : MvxWpfControl
    {

        public NavigationBarControl() => InitializeComponent();

        private void ProfileButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ProfileMenu.IsOpen = true;
        }

        private void ProfileButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!ProfileMenu.IsMouseOver)
            {
                ProfileMenu.IsOpen = false;
            }
        }

        private void ProfileMenu_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var x = Mouse.DirectlyOver;
            if (!ProfileButton.IsMouseOver && Mouse.DirectlyOver != null)
            {
                ProfileMenu.IsOpen = false;
            }
        }
    }
}