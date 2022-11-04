using Peernet.Browser.Application.ViewModels;
using System.Linq;
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
            var geoPoints = ((MainViewModel)DataContext).Footer.PeerStatuses.Select(ps => ps.GetGeoIP()).ToList();
            new PeersMapWindow(geoPoints).Show();
        }
    }
}