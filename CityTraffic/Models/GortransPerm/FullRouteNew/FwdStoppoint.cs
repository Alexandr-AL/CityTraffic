using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.FullRouteNew
{
    public class FwdStoppoint : Base.Stoppoint
    {
        [JsonPropertyName("stoppointId")]
        public override int StoppointId { get; set; }

        [JsonPropertyName("stoppointName")]
        public override string StoppointName { get; set; }

        [JsonPropertyName("note")]
        public override string Note { get; set; }

        [JsonPropertyName("course")]
        public int Course { get; set; }

        [JsonPropertyName("labelXOffset")]
        public int LabelXOffset { get; set; }

        [JsonPropertyName("labelYOffset")]
        public int LabelYOffset { get; set; }

        [JsonPropertyName("location")]
        public override string Location { get; set; }
    }
}
