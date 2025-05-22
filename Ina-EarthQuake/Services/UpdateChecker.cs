using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Ina_EarthQuake;
using Microsoft.UI.Xaml.Controls;

namespace Ina_EarthQuake.Services
{
    public static class UpdateChecker
    {
        private static readonly string latestUrl = "https://raw.githubusercontent.com/fakhrif/Ina-EarthQuake/master/latest.json?nocache=\" + Guid.NewGuid();";
        private static readonly Version currentVersion = new("1.0.5.0");

        public static async Task CheckForUpdateAsync()
        {
            try
            {
                using HttpClient client = new();
                Debug.WriteLine("Result cek update: " + client);
                string json = await client.GetStringAsync(latestUrl);
                Debug.WriteLine("Result cek update: " + json);
                var info = JsonSerializer.Deserialize(json, UpdateInfoJsonContext.Default.UpdateInfo);



                if (info != null && Version.TryParse(info.Version, out var latestVersion))
                {
                    if (latestVersion > currentVersion)
                    {
                        bool agreeToUpdate = await ShowUpdateDialogAsync(info.Version, info.Changelog);
                        if (agreeToUpdate)
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = info.Url,
                                Arguments = "/VERYSILENT /NORESTART",
                                UseShellExecute = true
                            });
                            Environment.Exit(0);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Gagal cek update: " + ex.Message);
            }
        }

        public static async Task<bool> ShowUpdateDialogAsync(string version, string changelog)
        {
            var dialog = new ContentDialog
            {
                Title = $"Versi baru tersedia: {version}",
                Content = $"Changelog:\n{changelog}",
                PrimaryButtonText = "Update Sekarang",
                CloseButtonText = "Nanti Saja",
                XamlRoot = App.m_window?.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }


        public class UpdateInfo
        {
            public string? Version { get; set; } = "";
            public string? Url { get; set; } = "";
            public string Changelog { get; set; } = "";
        }
    }
}