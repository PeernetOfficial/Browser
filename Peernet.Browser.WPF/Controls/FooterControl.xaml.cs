using MvvmCross.Plugin.Control.Platforms.Wpf;
using Peernet.Browser.Application.ViewModels;
using System.Linq;
using System.Windows;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class FooterControl : MvxWpfControl
    {
        public FooterControl() => InitializeComponent();

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var downloads = ((FooterViewModel)ViewModel).DownloadManager.ActiveFileDownloads;
            if (DownloadsList.Visibility == Visibility.Collapsed)
            {
                DownloadsList.Visibility = Visibility.Visible;
                DownloadsToggleButton.FontSize = 12;
                DownloadsToggleButton.Content = "Hide";
                CollapsedDownloadsText.Visibility = Visibility.Collapsed;
            }
            else
            {
                DownloadsList.Visibility = Visibility.Collapsed;
                DownloadsToggleButton.Content = "Show";
                DownloadsToggleButton.FontSize = 10;
                FileNameText.Text =
                    $"{downloads.FirstOrDefault().File.Name}...";
                if (downloads.Count > 1)
                {
                    FilesCounterText.Text = $" (+{downloads.Count - 1} files) ";
                }
                else
                {
                    FilesCounterText.Visibility = Visibility.Collapsed;
                }
                DownloadingText.Text = "downloading";
                CollapsedDownloadsText.Visibility = Visibility.Visible;
            }
        }
    }
}