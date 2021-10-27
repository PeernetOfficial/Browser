using MvvmCross.Plugin.Control.Platforms.Wpf;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ControlButtons.xaml
    /// </summary>
    public partial class ButtonsControl : MvxWpfControl
    {
        public ButtonsControl() => InitializeComponent();

        private void MvxWpfControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (System.Windows.Application.Current.MainWindow.WindowState != System.Windows.WindowState.Maximized)
            {
                System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}