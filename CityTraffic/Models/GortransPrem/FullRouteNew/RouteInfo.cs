using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityTraffic.Models.GortransPrem.FullRouteNew
{
    public class RouteInfo
    {
        [JsonPropertyName("routeId")]
        public string RouteId { get; set; }

        [JsonPropertyName("fwdStoppoints")]
        public List<FwdStoppoint> FwdStoppoints { get; set; }

        [JsonPropertyName("bkwdStoppoints")]
        public List<BkwdStoppoint> BkwdStoppoints { get; set; }

        [JsonPropertyName("twoStoppoints")]
        public List<object> TwoStoppoints { get; set; }

        [JsonPropertyName("threeStoppoints")]
        public List<object> ThreeStoppoints { get; set; }

        [JsonPropertyName("fourStoppoints")]
        public List<object> FourStoppoints { get; set; }

        [JsonPropertyName("fiveStoppoints")]
        public List<object> FiveStoppoints { get; set; }

        [JsonPropertyName("fwdTrackGeom")]
        public string FwdTrackGeom { get; set; }

        [JsonPropertyName("bkwdTrackGeom")]
        public string BkwdTrackGeom { get; set; }
    }
}
