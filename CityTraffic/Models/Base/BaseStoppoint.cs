using CityTraffic.Models.Base.Interfaces;

namespace CityTraffic.Models.Base
{
    public abstract class BaseStoppoint : IStoppoint
    {
        public virtual int StoppointId { get; set; }

        public virtual string StoppointName { get; set; }

        public virtual string Location { get; set; }

        public virtual string Note { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not BaseStoppoint other) return false;

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
