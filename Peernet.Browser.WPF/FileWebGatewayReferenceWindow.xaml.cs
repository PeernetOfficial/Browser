using DevExpress.Mvvm.Native;
using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Managers;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.WPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for FileWebGatewayReferenceWindow.xaml
    /// </summary>
    public partial class FileWebGatewayReferenceWindow : PeernetWindow
    {
        private DateTime deadline = new DateTime(2022, 12, 29, 12, 0, 0);

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

            SetupSubdomaindEventGateway();
        }

        public ApiFile File { get; set; }

        public Uri[] WebGatewayResourceUris => GetWebGatewayResourceUris();

        public Uri XmasWebGatewayResourceUris => GetXmasWebGatewayResourceUris();

        public Uri NewYearsEveWebGatewayResourceUris => GetNewYearsEveWebGatewayResourceUris();

        private void CopyLinkToClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            var webGatewayUri = ((FrameworkElement)e.OriginalSource)?.DataContext as Uri;
            Clipboard.SetText(webGatewayUri?.ToString());
            App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications.Add(new Notification("Copied to clipboard!"));
        }

        private void CopyNewYearsEveLinkToClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetNewYearsEveWebGatewayResourceUris()?.ToString());
            App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications.Add(new Notification("Copied New Year's Eve Card link to clipboard!"));
        }

        private void CopyXmasLinkToClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetXmasWebGatewayResourceUris()?.ToString());
            App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications.Add(new Notification("Copied X-Mas Card link to clipboard!"));
        }

        private Uri GetNewYearsEveWebGatewayResourceUris()
        {
            var domain = App.ServiceProvider.GetRequiredService<ISettingsManager>().NewYearsEveWebGatewayDomains;

            return new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = domain,
                Path = $"{Convert.ToHexString(File?.NodeId)}/{Convert.ToHexString(File?.Hash)}"
            }.Uri;
        }

        private Uri[] GetWebGatewayResourceUris()
        {
            var domains = App.ServiceProvider.GetRequiredService<ISettingsManager>().WebGatewayDomains;

            return domains?.Select(domain =>
                new UriBuilder
                {
                    Scheme = Uri.UriSchemeHttps,
                    Host = domain,
                    Path = $"{Convert.ToHexString(File?.NodeId)}/{Convert.ToHexString(File?.Hash)}"
                }.Uri).ToArray();
        }

        private Uri GetXmasWebGatewayResourceUris()
        {
            var domain = App.ServiceProvider.GetRequiredService<ISettingsManager>().XmasWebGatewayDomains;

            return new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = domain,
                Path = $"{Convert.ToHexString(File?.NodeId)}/{Convert.ToHexString(File?.Hash)}"
            }.Uri;
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

        private void SetupSubdomaindEventGateway()
        {
            if (File.Format is HighLevelFileType.Video)
            {
                if (DateTime.Now <= deadline)
                {
                    xmasPanel.Visibility = Visibility.Visible;
                    newYearsEvePanel.Visibility = Visibility.Collapsed;
                }
                else if (DateTime.Now > deadline)
                {
                    newYearsEvePanel.Visibility = Visibility.Visible;
                    xmasPanel.Visibility = Visibility.Collapsed;
                }
            }
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
    }
}