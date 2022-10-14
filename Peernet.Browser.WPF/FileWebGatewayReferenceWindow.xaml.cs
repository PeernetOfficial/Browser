using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.WPF;
using System.Windows.Input;
using System.Windows;
using System;
using Peernet.Browser.Application.Managers;
using Peernet.SDK.Models.Presentation.Footer;
using Microsoft.Extensions.DependencyInjection;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for FileWebGatewayReferenceWindow.xaml
    /// </summary>
    public partial class FileWebGatewayReferenceWindow : PeernetWindow
    {
        public ApiFile File { get; set; }

        public Uri WebGatewayResourceUrl => GetWebGatewayResourceUrl();

        public FileWebGatewayReferenceWindow(ApiFile file)
        {
            Initialized += Window_Initialized;
            ContentRendered += Window_ContentRendered;
            Owner = App.Current.MainWindow;
            WindowStartupLocation = App.Current.MainWindow.WindowStartupLocation;

            InitializeComponent();
            MouseDown += Window_MouseDown;
            DataContext = this;
            File = file;
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
            Clipboard.SetText(WebGatewayResourceUrl.ToString());
            App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications.Add(new Notification("Copied to clipboard!"));
        }

        private Uri GetWebGatewayResourceUrl()
        {
            return new UriBuilder()
            {
                Scheme = Uri.UriSchemeHttps,
                Host = "peer.ae",
                Path = $"{Convert.ToHexString(File?.NodeId)}/{Convert.ToHexString(File?.Hash)}"
            }.Uri;
        }
    }
}
