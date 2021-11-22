using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class MapModel
    {
        public double Height { get; set; }

        public double Width { get; set; }

        public List<GeoPoint> ConvertGeoPointsToMapScale(string geoIPs)
        {
            var geoPoints = ParseGeoIPs(geoIPs);
            return geoPoints.Select(Convert).ToList();
        }

        private GeoPoint Convert(GeoPoint point)
        {
            var x = (point.Longitude + 180) * (Width / 360);
            var y = (point.Latitude + 90) * (Height / 180);

            return new GeoPoint(y, x);
        }

        private List<GeoPoint> ParseGeoIPs(string geoIPs)
        {
            var geoPoints = new List<GeoPoint>();
            if (geoIPs == null)
            {
                return geoPoints;
            }

            var points = geoIPs.Split("\n");

            foreach (var point in points)
            {
                var latitudeLongitude = point.Split(",");
                geoPoints.Add(new GeoPoint(double.Parse(latitudeLongitude[0]), double.Parse(latitudeLongitude[1])));
            }

            return geoPoints;
        }
    }
}