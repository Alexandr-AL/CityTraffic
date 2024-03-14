namespace CityTraffic.Models.Entities
{
    public class EntityStoppoint : Base.Stoppoint
    {
        //public override int StoppointId { get; set; }

        //public override string StoppointName { get; set; }

        //public override string Location { get; set; }

        //public override string Note { get; set; }

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
