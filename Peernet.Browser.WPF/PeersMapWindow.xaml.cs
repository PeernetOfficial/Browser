using DevExpress.Xpf.Map;
using Peernet.SDK.WPF;
using System.Collections.Generic;
using System.Windows;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for PeersMapWindow.xaml
    /// </summary>
    public partial class PeersMapWindow : PeernetWindow
    {
        private readonly List<SDK.Models.Presentation.Home.GeoPoint> geoPoints;

        public PeersMapWindow(List<SDK.Models.Presentation.Home.GeoPoint> geoPoints)
        {
            InitializeComponent();
            this.geoPoints = geoPoints;
            Loaded += PeersMapWindow_Loaded;
        }

        private void PeersMapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var point in geoPoints)
            {
                if (point != null)
                {
                    var item = new MapPushpin()
                    {
                        Location = new GeoPoint(point.Latitude, point.Longitude)
                    };

                    mapItems.Items.Add(item);
                }
            }
        }

        private void OpenStreetMapDataProvider_WebRequest(object sender, MapWebRequestEventArgs e)
        {
            e.UserAgent = "Peernet Browser";
        }
    }
}
