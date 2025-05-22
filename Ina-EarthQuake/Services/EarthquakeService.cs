using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Ina_EarthQuake.Models;

namespace Ina_EarthQuake.Services
{
    public class EarthquakeService
    {
        private static readonly HttpClient client = new();

        static EarthquakeService()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Ina-EarthQuake-App/1.0");
        }

        public static async Task<EarthquakeInfo?> FetchEarthquakeData()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://data.bmkg.go.id/DataMKG/TEWS/autogempa.json");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                using JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                var gempa = jsonDocument.RootElement.GetProperty("Infogempa").GetProperty("gempa");

                return new EarthquakeInfo
                {
                    Datetime = gempa.GetProperty("DateTime").GetString() ?? "No Data",
                    Tanggal = gempa.GetProperty("Tanggal").GetString() ?? "No Data",
                    Jam = gempa.GetProperty("Jam").GetString() ?? "No Data",
                    Magnitude = gempa.GetProperty("Magnitude").GetString() ?? "No Data",
                    Kedalaman = gempa.GetProperty("Kedalaman").GetString() ?? "No Data",
                    Wilayah = gempa.GetProperty("Wilayah").GetString() ?? "No Data",
                    Lintang = gempa.GetProperty("Lintang").GetString() ?? "No Data",
                    Bujur = gempa.GetProperty("Bujur").GetString() ?? "No Data",
                    Potensi = gempa.GetProperty("Potensi").GetString() ?? "No Data",
                    Dirasakan = gempa.GetProperty("Dirasakan").GetString() ?? "No Data",
                    Shakemap = gempa.GetProperty("Shakemap").GetString() ?? "No Data"
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fetch Error: " + ex.ToString());
                return null;
            }
        }

        public static async Task<List<EarthquakeInfo>?> FetchEarthquakeHistory()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://data.bmkg.go.id/DataMKG/TEWS/gempadirasakan.json");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                using JsonDocument jsonDocument = JsonDocument.Parse(responseBody);

                var gempaRoot = jsonDocument.RootElement.GetProperty("Infogempa").GetProperty("gempa");
                List<EarthquakeInfo> earthquakeList = [];

                //Debug.WriteLine(gempaRoot);

                // Iterasi setiap objek di dalam "gempa"
                foreach (var data in gempaRoot.EnumerateArray())
                {
                    var gempaData = data;
                    //Debug.WriteLine(gempaData);

                    earthquakeList.Add(new EarthquakeInfo
                    {
                        Tanggal = gempaData.GetProperty("Tanggal").GetString() ?? "No Data",
                        Jam = gempaData.GetProperty("Jam").GetString() ?? "No Data",
                        Magnitude = gempaData.GetProperty("Magnitude").GetString() ?? "No Data",
                        Kedalaman = gempaData.GetProperty("Kedalaman").GetString() ?? "No Data",
                        Wilayah = gempaData.GetProperty("Wilayah").GetString() ?? "No Data",
                        Lintang = gempaData.GetProperty("Lintang").GetString() ?? "No Data",
                        Bujur = gempaData.GetProperty("Bujur").GetString() ?? "No Data",
                        Potensi = "No Data",
                        Dirasakan = gempaData.GetProperty("Dirasakan").GetString() ?? "No Data",
                        Shakemap = "No Data"
                    });
                }

                return earthquakeList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching data: {ex.Message}");
                return null;
            }
        }
    }
}