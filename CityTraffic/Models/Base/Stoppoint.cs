using CityTraffic.Models.Interfaces;

namespace CityTraffic.Models.Base
{
    public class Stoppoint : IStoppoint
    {
        public virtual int StoppointId { get; set; }

        public virtual string StoppointName { get; set; }

        public virtual string Note { get; set; }

        public virtual string Location { get; set; }
    }
}
