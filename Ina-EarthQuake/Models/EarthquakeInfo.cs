

using Ina_EarthQuake.Services;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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
        [JsonConverter(typeof(StringToDoubleJsonConverter))]
        public double? Magnitude { get; set; }
        public string FormattedMagnitude => Magnitude?.ToString("F1", CultureInfo.InvariantCulture) ?? "N/A";

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


        public string CleanWilayahTitle
        {
            get
            {
                if (string.IsNullOrEmpty(Wilayah))
                {
                    return "Informasi Wilayah Tidak Tersedia";
                }

                if (Wilayah.Contains("Pusat gempa berada di"))
                {
                    return Wilayah;
                }

                else
                {
                    TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
                    string titleCase = textInfo.ToTitleCase(Wilayah.ToLower());

                    string spaced = Regex.Replace(titleCase, "(\\B[A-Z])", " $1");

                    string final = spaced.Replace("-", ", ");

                    return final;
                }
            }
        }

    }
}
