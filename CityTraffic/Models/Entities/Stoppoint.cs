using CityTraffic.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CityTraffic.Models.Entities
{
    public class Stoppoint : IEntity, IStoppoint
    {
        public int Id { get;}
        public int StoppointId { get; set; }
        public string? StoppointName { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public bool Favorites { get; set; } = false;
    }
}
