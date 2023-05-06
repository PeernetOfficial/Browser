﻿using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.WPF.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.IO;
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
            var dataTransferManager = App.ServiceProvider.GetRequiredService<IDataTransferManager>();
            var downloadClient = App.ServiceProvider.GetRequiredService<IDownloadClient>();
            var filePath = Path.Combine(App.Settings.DownloadPath, UtilityHelper.StripInvalidWindowsCharactersFromFileName(model.File.Name));
            var download = new SDK.Models.Presentation.Download(downloadClient, model.File, filePath);
            var param = new FilePreviewViewModelParameter(model.File, async () => await dataTransferManager.QueueUp(download), "Download");
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

        private async void AddDirectoryTab(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var directoryViewModel = App.ServiceProvider.GetRequiredService<DirectoryViewModel>();
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var model = (DownloadModel)cellData.RowData.Row;
            await directoryViewModel.AddTab(model.File.NodeId);
            directoryViewModel.Navigate.Invoke();
            e.Handled = true;
        }

        private void AddMergedDirectoryTab(object sender, RoutedEventArgs e)
        {
            var directoryViewModel = App.ServiceProvider.GetRequiredService<DirectoryViewModel>();
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var model = (DownloadModel)cellData.RowData.Row;
            directoryViewModel.AddMergedTab(model.File.Hash);
            directoryViewModel.Navigate.Invoke();
            e.Handled = true;
        }
    }
}