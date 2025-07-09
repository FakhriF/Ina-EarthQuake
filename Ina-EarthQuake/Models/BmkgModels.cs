

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ina_EarthQuake.Models
{
    public class BmkgInfoGempa<T>
    {
        [JsonPropertyName("Infogempa")]
        public T? InfoGempa { get; set; }
    }

    public class AutoGempa
    {
        [JsonPropertyName("gempa")]
        public EarthquakeInfo? Gempa { get; set; }
    }

    public class GempaHistory
    {
        [JsonPropertyName("gempa")]
        public List<EarthquakeInfo>? Gempa { get; set; }
    }
}
