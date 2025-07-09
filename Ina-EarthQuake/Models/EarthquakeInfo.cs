

using System.Text.Json.Serialization;

namespace Ina_EarthQuake.Models
{
    public class EarthquakeInfo
    {
        [JsonPropertyName("DateTime")]
        public string? DateTime { get; set; }
        [JsonPropertyName("Tanggal")]
        public string? Tanggal { get; set; }
        [JsonPropertyName("Jam")]
        public string? Jam { get; set; }
        [JsonPropertyName("Magnitude")]
        public string? Magnitude { get; set; }
        [JsonPropertyName("Kedalaman")]
        public string? Kedalaman { get; set; }
        [JsonPropertyName("Wilayah")]
        public string?   Wilayah { get; set; }
        [JsonPropertyName("Lintang")]
        public string? Lintang { get; set; }
        [JsonPropertyName("Bujur")]
        public string? Bujur { get; set; }
        [JsonPropertyName("Potensi")]
        public string? Potensi { get; set; }
        [JsonPropertyName("Dirasakan")]
        public string? Dirasakan { get; set; }
        [JsonPropertyName("Shakemap")]
        public string? Shakemap { get; set; }

    }
}
