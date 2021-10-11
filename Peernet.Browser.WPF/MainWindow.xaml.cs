using MvvmCross.Platforms.Wpf.Views;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MouseDown += Window_MouseDown;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN"); ;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-EN");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
    }
}