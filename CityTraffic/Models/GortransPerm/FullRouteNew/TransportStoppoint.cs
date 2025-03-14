﻿using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.FullRouteNew
{
    public class TransportStoppoint
    {
        [JsonPropertyName("stoppointId")]
        public int StoppointId { get; set; }

        [JsonPropertyName("stoppointName")]
        public string StoppointName { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("course")]
        public int Course { get; set; }

        [JsonPropertyName("labelXOffset")]
        public int LabelXOffset { get; set; }

        [JsonPropertyName("labelYOffset")]
        public int LabelYOffset { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }
    }
}
