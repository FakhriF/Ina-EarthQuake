using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ina_EarthQuake;
using Microsoft.UI.Xaml.Controls;

namespace Ina_EarthQuake.Services
{
    public class UpdateService
    {
        private readonly HttpClient _client = new();
        private readonly string latestUrl = "https://raw.githubusercontent.com/fakhrif/Ina-EarthQuake/master/latest.json?nocache=\" + Guid.NewGuid();";
        private readonly Version currentVersion = new("1.0.8.0");

        public async Task<UpdateInfo?> GetUpdateInfoIfAvailableAsync()
        {
            try
            {
                string json = await _client.GetStringAsync(latestUrl);
                var info = JsonSerializer.Deserialize(json, UpdateInfoJsonContext.Default.UpdateInfo);


                if (info != null && Version.TryParse(info.Version, out var latestVersion))
                {
                    if (latestVersion > currentVersion)
                    {
                        return info;

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ERROR] Gagal cek update: " + ex.Message);
            }
            return null;
        }

        public class UpdateInfo
        {
            public string? Version { get; set; }
            public string? Url { get; set; }
            public string? Changelog { get; set; }
        }
    }

    [JsonSerializable(typeof(UpdateService.UpdateInfo))]
    internal partial class UpdateInfoJsonContext : JsonSerializerContext
    {

    }
}