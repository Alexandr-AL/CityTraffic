namespace CityTraffic.Models.Entities
{
    public class StoppointEntity
    {
        public int StoppointId { get; set; }

        public string StoppointName { get; set; } = string.Empty;

        public Location Location { get; set; }

        public string Note { get; set; } = string.Empty;

        public bool IsFavorite { get; set; }

        public List<TransportRouteEntity> Routes { get; set; } = [];


        public override bool Equals(object obj)
        {
            if (obj == null || obj is not StoppointEntity other) return false;

            return StoppointId == other.StoppointId &&
                   StoppointName == other.StoppointName &&
                   Location.Latitude == other.Location.Latitude &&
                   Location.Longitude == other.Location.Longitude&&
                   Note == other.Note;
        }

        public override int GetHashCode() => StoppointId.GetHashCode();
    }
}
