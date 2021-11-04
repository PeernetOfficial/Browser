using MvvmCross.Plugin.Control.Platforms.Wpf;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Peernet.Browser.Models.Domain.Download;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class FooterControl : MvxWpfControl
    {
        private readonly IDownloadManager downloadManager;

        public FooterControl()
        {
            InitializeComponent();
            downloadManager = ((FooterViewModel)ViewModel).DownloadManager;
            downloadManager.downloadsChanged += OnDownloadsChange;
        }

        private void Downloads_Toggle(object sender, RoutedEventArgs e)
        {
            if (DownloadsList.Visibility == Visibility.Collapsed)
            {
                DownloadsList.Visibility = Visibility.Visible;
                CollapsedDownloadsText.Visibility = Visibility.Collapsed;
            }
            else
            {
                DownloadsList.Visibility = Visibility.Collapsed;
                CollapsedDownloadsText.Visibility = Visibility.Visible;
            }
        }

        private void OnDownloadsChange(object sender, EventArgs args)
        {
            var activeDownloads = GetActiveDownloads();
            var completedDownloads = GetCompletedDownloads();
            var pausedDownloads = GetPausedDownloads();

            if (activeDownloads.Count != 0)
            {
                DownloadingText.Text = "downloading";
                SetCollapsedDownloadsLabel(activeDownloads);
            }
            else if (completedDownloads.Count != 0)
            {
                DownloadingText.Text = "downloaded";
                SetCollapsedDownloadsLabel(completedDownloads);
            }
            else if (pausedDownloads.Count != 0)
            {
                DownloadingText.Text = "paused";
                SetCollapsedDownloadsLabel(pausedDownloads);
            }
        }

        private void SetCollapsedDownloadsLabel(IReadOnlyCollection<DownloadModel> downloads)
        {
            if (downloads.Count != 0)
            {
                var fileName = downloads.First().File.Name;
                if (downloads.Count == 1)
                {
                    FilesCounterText.Visibility = Visibility.Collapsed;
                    FileNameText.Text = $"{fileName}";

                }
                else if (downloads.Count > 1)
                {
                    FilesCounterText.Visibility = Visibility.Visible;
                    FilesCounterText.Text = $" (+{downloads.Count - 1} files) ";
                    FileNameText.Text = $"{fileName}...";
                }
            }
        }

        private List<DownloadModel> GetActiveDownloads()
        {
            return downloadManager.ActiveFileDownloads.Where(d => !d.IsCompleted && d.Status != DownloadStatus.DownloadPause).ToList();
        }

        private List<DownloadModel> GetCompletedDownloads()
        {
            return downloadManager.ActiveFileDownloads.Where(d => d.IsCompleted).ToList();
        }

        private List<DownloadModel> GetPausedDownloads()
        {
            return downloadManager.ActiveFileDownloads.Where(d => d.Status == DownloadStatus.DownloadPause).ToList();
        }
    }
}