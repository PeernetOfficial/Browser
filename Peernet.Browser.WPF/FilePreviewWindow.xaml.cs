using Peernet.Browser.Application.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for FilePreviewWindow.xaml
    /// </summary>
    public partial class FilePreviewWindow : Window
    {
        public FilePreviewWindow(FilePreviewViewModel dataContext)
        {
            ContentRendered += Window_ContentRendered;
            Initialized += Window_Initialized;
            InitializeComponent();
            DataContext = dataContext;
            MouseDown += Window_MouseDown;

            WindowStartupLocation = App.Current.MainWindow.WindowStartupLocation;
            //Owner = App.Current.MainWindow;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.Topmost = false;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Topmost = true;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}