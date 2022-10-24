using Peernet.Browser.Application.ViewModels;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class FooterControl : UserControl
    {
        public FooterControl()
        {
            InitializeComponent();
        }

        private void GlobeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new PeersMapWindow(((MainViewModel)DataContext).Footer).Show();
        }
    }
}