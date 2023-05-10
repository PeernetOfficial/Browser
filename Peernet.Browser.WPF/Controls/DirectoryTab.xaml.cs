using Peernet.Browser.Application.Interfaces;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                new ResultsSharingWindow("directory", fileModel).Show();
            }
        }
    }
}
