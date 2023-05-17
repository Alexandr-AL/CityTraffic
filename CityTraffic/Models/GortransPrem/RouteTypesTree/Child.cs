using System.Text.Json.Serialization;
using CityTraffic.Models.Interfaces;

namespace CityTraffic.Models.GortransPrem.RouteTypesTree
{
    public class Child : ITransportRoute
    {
        [JsonPropertyName("routeId")]
        public string RouteId { get; set; }

        [JsonPropertyName("routeNumber")]
        public string RouteNumber { get; set; }

        [JsonPropertyName("routeTypeId")]
        public int RouteTypeId { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("isLazy")]
        public bool IsLazy { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }

        [JsonPropertyName("noLink")]
        public bool NoLink { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}