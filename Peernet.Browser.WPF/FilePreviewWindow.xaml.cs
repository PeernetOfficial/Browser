using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Models.Presentation.Footer;
using System.Windows;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for FilePreviewWindow.xaml
    /// </summary>
    public partial class FilePreviewWindow : MvxWindow
    {
        private readonly DownloadModel model;

        public FilePreviewWindow(DownloadModel model)
        {
            InitializeComponent();
            this.model = model;
            Content = model;
        }

        private void Minimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Download_OnClick(object sender, RoutedEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IDownloadManager>().QueueUpDownload(model);
        }
    }
}