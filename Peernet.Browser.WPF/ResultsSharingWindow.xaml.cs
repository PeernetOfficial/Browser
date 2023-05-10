using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Managers;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.WPF;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for ResultsSharingWindow.xaml
    /// </summary>
    public partial class ResultsSharingWindow : PeernetWindow
    {
        private string view;
        public FileModel FileModel { get; set; }

        public string PeernetSchemaUri => $"peernet://{view}?hash={Convert.ToHexString(FileModel.Hash)}&node={Convert.ToHexString(FileModel.NodeId)}"; 

        public ResultsSharingWindow(string view, FileModel fileModel)
        {
            Initialized += Window_Initialized;
            ContentRendered += Window_ContentRendered;
            Owner = App.Current.MainWindow;
            WindowStartupLocation = App.Current.MainWindow.WindowStartupLocation;

            InitializeComponent();
            MouseDown += Window_MouseDown;
            DataContext = this;
            this.view = view;
            FileModel = fileModel;
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

        private void CopyLinkToClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PeernetSchemaUri);
            App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications.Add(new Notification("Copied to clipboard!"));
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var processInfo = new ProcessStartInfo(PeernetSchemaUri.ToString());
            processInfo.UseShellExecute = true;
            Process.Start(processInfo);
            e.Handled = true;
        }
    }
}
