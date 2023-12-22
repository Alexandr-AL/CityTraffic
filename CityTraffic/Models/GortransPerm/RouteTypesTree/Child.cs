using CityTraffic.Models.Base;
using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPerm.RouteTypesTree
{
    public class Child : BaseTransportRoute
    {
        [JsonPropertyName("routeId")]
        public override string RouteId { get => base.RouteId; set => base.RouteId = value; }

        [JsonPropertyName("routeNumber")]
        public override string RouteNumber { get => base.RouteNumber; set => base.RouteNumber = value; }

        [JsonPropertyName("title")]
        public override string Title { get => base.Title; set => base.Title = value; }

        [JsonPropertyName("routeTypeId")]
        public override int RouteTypeId { get => base.RouteTypeId; set => base.RouteTypeId = value; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("isLazy")]
        public bool IsLazy { get; set; }

        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }

        [JsonPropertyName("noLink")]
        public bool NoLink { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}