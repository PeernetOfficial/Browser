using System;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Presentation.Footer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            var file = (ApiFile)((FrameworkElement)e.OriginalSource).DataContext;
            var link = $"peer://{Convert.ToHexString(file.NodeId)}/{Convert.ToHexString(file.Hash)}/{file.Folder}/{file.Name}";
            Clipboard.SetText(link);
            GlobalContext.Notifications.Add(new Notification { Text = "Copied to clipboard!" });
        }

        private void OpenFilePreview_OnClick(object sender, MouseButtonEventArgs e)
        {
            ShowFilePreview(e, false);
        }

        private static void ShowFilePreview(RoutedEventArgs e, bool isEditable)
        {
            var file = (ApiFile)((FrameworkElement)e.OriginalSource).DataContext;
            var model = new DownloadModel(file);
            var preview = new FilePreviewWindow(model, isEditable);
            preview.Show();
        }
    }
}