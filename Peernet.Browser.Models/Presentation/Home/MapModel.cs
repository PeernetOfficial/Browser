using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class MapModel
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public MvxObservableCollection<MapPoint> Points { get; } = new MvxObservableCollection<MapPoint>();

        public void Fill(IEnumerable<GeoPoint> points)
        {
            Points.Clear();
            points.Foreach(Add);
        }

        private void Add(GeoPoint point) => Points.Add(Map(point));

        private MapPoint Map(GeoPoint point)
        {
            var x = (point.Longitude + 180) * ((Width - 30) / 360);
            var y = (point.Latitude + 180) * ((Height - 30) / 360);
            return new MapPoint
            {
                Height = 10,
                Width = 10,
                X = x,
                Y = y
            };
        }
    }
}