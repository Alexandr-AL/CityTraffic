using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.RouteTypesTree
{
    public class Child : Base.TransportRoute
    {
        [JsonPropertyName("routeId")]
        public override string RouteId { get; set; }

        [JsonPropertyName("routeNumber")]
        public override string RouteNumber { get; set; }

        [JsonPropertyName("routeTypeId")]
        public override int RouteTypeId { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("isLazy")]
        public bool IsLazy { get; set; }

        [JsonPropertyName("title")]
        public override string Title { get; set; }

        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }

        [JsonPropertyName("noLink")]
        public bool NoLink { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}