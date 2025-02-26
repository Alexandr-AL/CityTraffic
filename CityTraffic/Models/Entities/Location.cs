using System.Globalization;
using System.Text.RegularExpressions;

namespace CityTraffic.Models.Entities
{
    public class Location
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Location(string coordinates)
        {
            (double lat, double lon) =  ParseCoordinates(coordinates);
            Latitude = lat;
            Longitude = lon;
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static (double Lat, double Lon) ParseCoordinates(string coord)
        {
            if (string.IsNullOrWhiteSpace(coord))
                return default;

            Regex regex = new Regex(@"[-+]?[0-9]*\.?[0-9]+");
            MatchCollection matches = regex.Matches(coord);

            if (matches.Count != 2)
                return default;

            double lat;
            double lon;

            try
            {
                lat = double.Parse(matches[0].Value, CultureInfo.InvariantCulture);
                lon = double.Parse(matches[1].Value, CultureInfo.InvariantCulture);
            }
            catch (ArgumentNullException)
            {
                return default;
            }
            catch (FormatException)
            {
                return default;
            }

            return (lat, lon);
        }
    }
}
