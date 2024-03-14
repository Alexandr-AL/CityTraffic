namespace CityTraffic.Models.Interfaces
{
    public interface IStoppoint
    {
        int StoppointId { get; set; }

        string StoppointName { get; set; }

        string Note { get; set; }

        string Location { get; set; }
    }
}
