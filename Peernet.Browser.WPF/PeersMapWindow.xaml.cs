using DevExpress.Xpf.Map;
using Peernet.SDK.WPF;
using System.Windows;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for PeersMapWindow.xaml
    /// </summary>
    public partial class PeersMapWindow : PeernetWindow
    {
        public PeersMapWindow()
        {
            InitializeComponent();
            Loaded += PeersMapWindow_Loaded;
        }

        private void PeersMapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //foreach(var peer in viewModel.Peers)
            //{
            //    var item = new MapPushpin()
            //    {
            //        Location = new GeoPoint(peer.Latitude, peer.Longitude)
            //    };
            //    mapItems.Items.Add(item);
            //}
        }

        private void OpenStreetMapDataProvider_WebRequest(object sender, MapWebRequestEventArgs e)
        {
            e.UserAgent = "Peernet Browser";
        }
    }
}
