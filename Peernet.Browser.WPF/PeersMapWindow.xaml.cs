using DevExpress.Xpf.Map;
using Peernet.Browser.Application.ViewModels;
using Peernet.SDK.Models.Domain.Status;
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
        private readonly FooterViewModel footerViewModel;

        public PeersMapWindow(FooterViewModel viewModel)
        {
            InitializeComponent();
            footerViewModel = viewModel;
            Loaded += PeersMapWindow_Loaded;

        }

        private void PeersMapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var status in footerViewModel.PeerStatuses)
            {
                var point = status.GetGeoIP();
                if (point != null)
                {
                    var item = new MapPushpin()
                    {
                        Location = new GeoPoint(point.Latitude, point.Longitude),
                        ToolTipPattern = $"{nameof(status.PeerId)}: {status.PeerId}"
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
