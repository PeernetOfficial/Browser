using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Presentation.Footer;
using System;
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
            var link = $"peer://{Convert.ToHexString(file.NodeId)}/{Convert.ToHexString(file.Hash)}/{file.Folder}/{file.Name}";
            Clipboard.SetText(link);
            GlobalContext.Notifications.Add(new Notification("Copied to clipboard!"));
        }
    }
}