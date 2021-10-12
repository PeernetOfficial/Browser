using System.Windows;
using System.Windows.Controls;
using Peernet.Browser.Models.Presentation.Footer;

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

        private void OpenPreview_OnClick(object sender, RoutedEventArgs e)
        {
            var downloadModel = (DownloadModel)((FrameworkElement)e.OriginalSource).DataContext;
            var previewWindow = new FilePreviewWindow(downloadModel);
            previewWindow.Show();
        }
    }
}