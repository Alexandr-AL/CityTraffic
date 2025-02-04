namespace CityTraffic.Models.Entities
{
    public class EntityStoppoint
    {
        public int StoppointId { get; set; }

        public string StoppointName { get; set; }

        public string Location { get; set; }

        public string Note { get; set; }

        public bool FavoriteStoppoint { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not EntityStoppoint other) return false;

            return StoppointId == other.StoppointId &&
                   StoppointName == other.StoppointName &&
                   Location == other.Location &&
                   Note == other.Note;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
