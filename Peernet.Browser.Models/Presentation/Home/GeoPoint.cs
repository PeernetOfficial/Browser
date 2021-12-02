namespace Peernet.Browser.Models.Presentation.Home
{
    public class GeoPoint
    {
        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Y
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public double Longitude { get; set; }
    }
}