using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Infrastructure.Services;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Collections.Generic;
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

        private void Pager_Loaded(object sender, RoutedEventArgs e)
        {
            pager.PageIndexChanged += Pager_OnChange;
            pager.PageSizeChanged += Pager_OnChange;
        }

        private async void Pager_OnChange(object sender, EventArgs e)
        {
            if (DataContext != null)
            {
                await (DataContext as ExploreViewModel)?.ReloadResults();
            }
        }

        private async void OpenUserProfile_PreviewLeftMouseButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var context = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var model = (DownloadModel)context.Row;
            var nodeId = model.File.NodeId;
            var userService = App.ServiceProvider.GetService<IUserService>();
            var downoadManager = App.ServiceProvider.GetService<IDownloadManager>();
            var plugs = App.ServiceProvider.GetService<IEnumerable<IPlayButtonPlug>>();
            var user = await userService.GetUser(nodeId);
            new UserProfileWindow(new(user, downoadManager, plugs)).Show();
        }
    }
}