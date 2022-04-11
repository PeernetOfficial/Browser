using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for SharedFilesDataGridControl.xaml
    /// </summary>
    public partial class SharedFilesDataGridControl : UserControl
    {
        public SharedFilesDataGridControl()
        {
            InitializeComponent();
        }

        private void CopyLinkToClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var file = ((VirtualFileSystemEntity)cellData.RowData.Row).File;

            Clipboard.SetText(CreateLink(file));
            App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications.Add(new Notification("Copied to clipboard!"));
        }

        private static string CreateLink(ApiFile file)
        {
            return Path.Join(@"peer://", Convert.ToHexString(file.NodeId), Convert.ToHexString(file.Hash), file.Folder, file.Name).Replace(@"\", "/");
        }

        private async void Open_OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var viewmodel = (DirectoryViewModel)DataContext;
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var entity = (VirtualFileSystemEntity)cellData.RowData.Row;
            if (entity is VirtualFileSystemCoreEntity coreTier)
            {
                await viewmodel.OpenCommand.ExecuteAsync(coreTier);
            }
            else
            {
                await viewmodel.StreamFileCommand.ExecuteAsync(entity);
            }
        }

        private async void OpenInfo_OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cellData = (EditGridCellData)((FrameworkElement)e.OriginalSource).DataContext;
            var entity = (VirtualFileSystemEntity)cellData.RowData.Row;
            var warehouseService = App.ServiceProvider.GetRequiredService<IWarehouseService>();
            var param = new FilePreviewViewModelParameter(entity.File, () => warehouseService.ReadPath(entity.File), "Save To File");
            var filePreviewViewModel = new FilePreviewViewModel();
            await filePreviewViewModel.Prepare(param);
            var previewWindow = new FilePreviewWindow(filePreviewViewModel);
            previewWindow.Show();
            Dispatcher.Invoke(() =>
            {
                previewWindow.Activate();
                previewWindow.Focus();
            });
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
    }
}