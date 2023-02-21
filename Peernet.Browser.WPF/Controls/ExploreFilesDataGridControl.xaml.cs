using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ExploreFilesDataGridControl.xaml
    /// </summary>
    public partial class ExploreFilesDataGridControl : UserControl
    {
        public ExploreFilesDataGridControl()
        {
            InitializeComponent();
        }

        private void Open_OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var model = (DownloadModel)cellData.RowData.Row;
            var downloadManager = App.ServiceProvider.GetRequiredService<IDownloadManager>();
            var param = new FilePreviewViewModelParameter(model.File, async () => await downloadManager.QueueUpDownload(model), "Download");
            var filePreviewViewModel = new FilePreviewViewModel();
            filePreviewViewModel.Prepare(param);
            new FilePreviewWindow(filePreviewViewModel).Show();
        }

        private void OpenFileWebGatewayReferenceWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var model = (DownloadModel)cellData.RowData.Row;
            new FileWebGatewayReferenceWindow(model.File).Show();
        }

        private void OpenPeersMap(object sender, RoutedEventArgs e)
        {
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var model = (DownloadModel)cellData.RowData.Row;
            new PeersMapWindow(model.GeoPoints).Show();
        }
    }
}