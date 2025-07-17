using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Ina_EarthQuake.Models;

namespace Ina_EarthQuake.Services
{
    public class EarthquakeService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _serializerOptions;

        public EarthquakeService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Ina-EarthQuake-App/1.0");
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        private async Task<T?> GetAsync<T> (string url)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseBody, _serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Failed to fetch or parse {url}: {ex.Message}");
                return default;
            }
        }

        public async Task<EarthquakeInfo?> FetchLatestEarthquakeAsync()
        {
            var result = await GetAsync<BmkgInfoGempa<AutoGempa>>("https://data.bmkg.go.id/DataMKG/TEWS/autogempa.json");
            return result?.InfoGempa?.Gempa;
        }

        public async Task<List<EarthquakeInfo>?> FetchFeltEarthquake()
        {
            var feltTask = GetAsync<BmkgInfoGempa<GempaHistory>>("https://data.bmkg.go.id/DataMKG/TEWS/gempadirasakan.json");

            await Task.WhenAll(feltTask);

            var feltEarthquakes = feltTask.Result?.InfoGempa?.Gempa ?? new List<EarthquakeInfo>();

            var dictFeltEartquakes = new Dictionary<string, EarthquakeInfo>();

            foreach (var quake in feltEarthquakes)
            {
                if (quake.DateTime != null && !dictFeltEartquakes.ContainsKey(quake.DateTime))
                {
                    dictFeltEartquakes.Add(quake.DateTime, quake);
                }
            }

            return dictFeltEartquakes.Values.OrderByDescending(q => DateTime.Parse(q.DateTime!)).ToList();
        }

        public async Task<List<EarthquakeInfo>?> FetchRecentEarthquake()
        {
            var recentTask = GetAsync<BmkgInfoGempa<GempaHistory>>("https://data.bmkg.go.id/DataMKG/TEWS/gempaterkini.json");

            await Task.WhenAll(recentTask);

            var recentEarthquakes = recentTask.Result?.InfoGempa?.Gempa ?? new List<EarthquakeInfo>();

            var dictrecentEarthquakes = new Dictionary<string, EarthquakeInfo>();

            foreach (var quake in recentEarthquakes)
            {
                if (quake.DateTime != null && !dictrecentEarthquakes.ContainsKey(quake.DateTime))
                {
                    dictrecentEarthquakes.Add(quake.DateTime, quake);
                }
            }

            return dictrecentEarthquakes.Values.OrderByDescending(q => DateTime.Parse(q.DateTime!)).ToList();
        }

        public async Task<List<EarthquakeInfo>?> FetchEarthquakeHistoryAsync()
        {
            var feltTask= GetAsync<BmkgInfoGempa<GempaHistory>>("https://data.bmkg.go.id/DataMKG/TEWS/gempadirasakan.json");
            var recentTask = GetAsync<BmkgInfoGempa<GempaHistory>>("https://data.bmkg.go.id/DataMKG/TEWS/gempaterkini.json");

            await Task.WhenAll(feltTask, recentTask);

            var feltEarthquakes = feltTask.Result?.InfoGempa?.Gempa ?? new List<EarthquakeInfo>();
            var recentEarthquakes = recentTask.Result?.InfoGempa?.Gempa ?? new List<EarthquakeInfo>();

            //Menyatukan data felt dan recent
            var combineQuakes = new Dictionary<string, EarthquakeInfo>();

            foreach(var quake in feltEarthquakes)
            {
                if (quake.DateTime != null && !combineQuakes.ContainsKey(quake.DateTime))
                {
                    combineQuakes.Add(quake.DateTime, quake);
                }
            }

            foreach (var quake in recentEarthquakes)
            {
                if (quake.DateTime != null && !combineQuakes.ContainsKey(quake.DateTime))
                {
                    combineQuakes.Add(quake.DateTime, quake);
                }
            }

            return combineQuakes.Values
                .OrderByDescending(q => DateTime.Parse(q.DateTime!))
                .ToList();
        }
    }
}