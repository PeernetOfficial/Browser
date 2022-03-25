using Peernet.Browser.Application.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for ChangleFileLocationWindow.xaml
    /// </summary>
    public partial class ChangleFileLocationWindow : Window
    {
        public ChangleFileLocationWindow(ChangeFileLocationViewModel dataContext)
        {
            Initialized += Window_Initialized;
            ContentRendered += Window_ContentRendered;
            Owner = App.Current.MainWindow;
            WindowStartupLocation = App.Current.MainWindow.WindowStartupLocation;

            InitializeComponent();
            DataContext = dataContext;
            MouseDown += Window_MouseDown;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.Topmost = false;
            this.Focus();
            Owner = null;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Activate();
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

        private void Maximize_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Minimize_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            ((ChangeFileLocationViewModel)DataContext).SelectCommand.ExecuteAsync();
            Close();
        }

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }
        }
    }
}