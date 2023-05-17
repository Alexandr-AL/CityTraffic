using System.Text.Json.Serialization;

namespace CityTraffic.Models.GortransPrem.RouteTypesTree
{
    public class TransportType
    {
        [JsonPropertyName("children")]
        public List<Child> Children { get; set; }

        [JsonPropertyName("routeTypeId")]
        public int RouteTypeId { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("hideCheckbox")]
        public bool HideCheckbox { get; set; }

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