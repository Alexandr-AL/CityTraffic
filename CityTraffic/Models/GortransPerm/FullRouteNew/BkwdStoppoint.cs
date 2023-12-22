using CityTraffic.Models.Base;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.FullRouteNew
{
    public class BkwdStoppoint : BaseStoppoint
    {
        [JsonPropertyName("stoppointId")]
        public override int StoppointId { get => base.StoppointId; set => base.StoppointId = value; }

        [JsonPropertyName("stoppointName")]
        public override string StoppointName { get => base.StoppointName; set => base.StoppointName = value; }

        [JsonPropertyName("location")]
        public override string Location { get => base.Location; set => base.Location = value; }

        [JsonPropertyName("note")]
        public override string Note { get => base.Note; set => base.Note = value; }

        [JsonPropertyName("course")]
        public int Course { get; set; }

        [JsonPropertyName("labelXOffset")]
        public int LabelXOffset { get; set; }

        [JsonPropertyName("labelYOffset")]
        public int LabelYOffset { get; set; }

    }
}
