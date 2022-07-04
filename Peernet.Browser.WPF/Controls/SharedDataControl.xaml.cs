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
    /// Interaction logic for SharedDataControl.xaml
    /// </summary>
    public partial class SharedDataControl : UserControl
    {
        public SharedDataControl()
        {
            InitializeComponent();
            Loaded += SharedDataControl_Loaded;
        }

        private async void GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            var model = e?.NewItem as DownloadModel;

            // This will require IComparable implementation
            if (model == SharedFiles.SelectedItem)
            {
                UnselectItem();
            }

            if (model != null)
            {
                Comments.Visibility = Visibility.Visible;
                var context = (SharedDataViewModel)((FrameworkElement)e.OriginalSource).DataContext;
                await context.GetCommentsCommand.ExecuteAsync(model);
            }
        }

        private void OpenFilePreview_OnClick(object sender, RoutedEventArgs e)
        {
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var model = (DownloadModel)cellData.RowData.Row;
            var downloadManager = App.ServiceProvider.GetRequiredService<IDownloadManager>();
            var param = new FilePreviewViewModelParameter(model.File, async () => await downloadManager.QueueUpDownload(model), "Download");
            var filePreviewViewModel = new FilePreviewViewModel();
            filePreviewViewModel.Prepare(param);
            new FilePreviewWindow(filePreviewViewModel).Show();
        }

        private void SharedDataControl_Loaded(object sender, RoutedEventArgs e)
        {
            UnselectItem();
        }

        private void UnselectItem()
        {
            SharedFiles.SelectedItem = null;
            Comments.Visibility = Visibility.Collapsed;
        }
    }
}