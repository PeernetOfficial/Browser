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
            var file = ((VirtualFileSystemEntity)((FrameworkElement)e.OriginalSource).DataContext).File;

            Clipboard.SetText(CreateLink(file));
            App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications.Add(new Notification("Copied to clipboard!"));
        }

        private static string CreateLink(ApiFile file)
        {
            return Path.Join(@"peer://", Convert.ToHexString(file.NodeId), Convert.ToHexString(file.Hash), file.Folder, file.Name).Replace(@"\", "/");
        }

        private async void Open_OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var entity = (VirtualFileSystemEntity)element.DataContext;
            if (entity is VirtualFileSystemCoreEntity coreTier)
            {
                var viewmodel = (DirectoryViewModel)DataContext;
                await viewmodel.OpenCommand.ExecuteAsync(coreTier);
            }
            else
            {
                var warehouseService = App.ServiceProvider.GetRequiredService<IWarehouseService>();
                var param = new FilePreviewViewModelParameter(entity.File, false, () => warehouseService.ReadPath(entity.File), "Save To File");
                var filePreviewViewModel = new FilePreviewViewModel();
                await filePreviewViewModel.Prepare(param);
                new FilePreviewWindow(filePreviewViewModel).Show();
            }
        }
    }
}