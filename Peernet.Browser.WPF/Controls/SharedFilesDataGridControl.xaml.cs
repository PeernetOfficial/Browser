using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Presentation.Footer;
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
            GlobalContext.Notifications.Add(new Notification("Copied to clipboard!"));
        }

        private string CreateLink(ApiFile file)
        {
            return Path.Join("peer://", Convert.ToHexString(file.NodeId), Convert.ToHexString(file.Hash), file.Folder, file.Name);
        }
    }
}