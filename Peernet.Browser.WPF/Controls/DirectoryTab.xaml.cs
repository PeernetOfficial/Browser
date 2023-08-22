using Peernet.Browser.Application.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for DirectoryTab.xaml
    /// </summary>
    public partial class DirectoryTab : UserControl
    {
        public DirectoryTab()
        {
            InitializeComponent();
        }

        private async void ShareDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = ((FrameworkElement)e.OriginalSource).DataContext;
            var shareableContent = dataContext as IShareableContent;
            if (shareableContent != null)
            {
                var fileModel = await shareableContent.CreateResultsSnapshot();
                new ResultsSharingWindow(fileModel).Show();
            }
        }
    }
}