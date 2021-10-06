using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Presentation;
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
            var file = (ApiBlockRecordFile)((FrameworkElement)e.OriginalSource).DataContext;
            var link = $"peer://{file.NodeId}/{file.Hash}/{file.Folder}/{file.Name}";
            Clipboard.SetText(link);
            GlobalContext.Notifications.Add(new Notification { Text = "Copied to clipboard!" });
        }
    }
}