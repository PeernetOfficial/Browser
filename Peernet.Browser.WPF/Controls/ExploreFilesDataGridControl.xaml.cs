using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Models.Presentation.Footer;
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
            pager.Loaded += Pager_Loaded;
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

        private void Pager_Loaded(object sender, RoutedEventArgs e)
        {
            pager.PageIndexChanged += Pager_PageIndexChanged;
            pager.PageSizeChanged += Pager_PageSizeChanged;
        }

        private async void Pager_PageIndexChanged(object sender, DevExpress.Xpf.Editors.DataPager.DataPagerPageIndexChangedEventArgs e)
        {
            if (DataContext != null)
            {
                await (DataContext as SearchTabElementViewModel)?.Refresh();
            }
        }

        private async void Pager_PageSizeChanged(object sender, DevExpress.Xpf.Editors.DataPager.DataPagerPageSizeChangedEventArgs e)
        {
            if (DataContext != null)
            {
                await (DataContext as SearchTabElementViewModel)?.Refresh();
            }
        }
    }
}