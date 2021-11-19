using Peernet.Browser.Models.Domain.Common;
using System;
using System.Collections.Generic;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class SearchResultRowModel
    {
        private bool isHovered;

        public SearchResultRowModel(ApiFile source)
        {
            File = source;
            EnumerationMember = (HealthType)3;
            Name = source.Name;
            Date = source.Date;
            Size = $"{source.Size}";
            SharedBy = source.SharedByCount;
            //FlameIsVisible = source.SharedByCount > 15;
            Points = ParseGeoIPs(source.SharedByGeoIP);
        }

        public DateTime Date { get; }
        public HealthType EnumerationMember { get; }

        public ApiFile File { get; }
        public bool FlameIsVisible { get; }
        public bool IsCompleted { get; }

        public bool IsHovered
        {
            get => isHovered;
            set
            {
                isHovered = value;
                if (IsHovered)
                {
                    OnHover?.Invoke(Points);
                }
            }
        }

        public string Name { get; }
        public Action<List<GeoPoint>> OnHover { get; set; }
        public List<GeoPoint> Points { get; }
        public int SharedBy { get; }
        public string Size { get; }

        public static DataGridSortingNameEnum Parse(string name)
        {
            switch (name)
            {
                case nameof(Name):
                    return DataGridSortingNameEnum.Name;

                case nameof(Date):
                    return DataGridSortingNameEnum.Date;

                case nameof(Size):
                    return DataGridSortingNameEnum.Size;

                case nameof(SharedBy):
                    return DataGridSortingNameEnum.Share;

                default:
                    return DataGridSortingNameEnum.None;
            }
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