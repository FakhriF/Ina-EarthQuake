using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Ina_EarthQuake.Models;
using System;
using Ina_EarthQuake;

namespace Ina_EarthQuake.Services
{
    using Microsoft.Windows.AppNotifications.Builder;
    using Microsoft.Windows.AppNotifications;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Threading;
    using Ina_EarthQuake.Models;
    using System;
    using Ina_EarthQuake;

    public class EarthquakePollingService
    {
        private Timer? _timer;
        private readonly EarthquakeInfo? _lastEarthquake;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);
        private readonly object _lock = new();

        public bool IsRunning { get; private set; } = false;

        public void Start()
        {
            if (IsRunning) return;

            _timer = new Timer(async _ => await CheckForUpdates(), null, TimeSpan.Zero, _interval);
            IsRunning = true;
        }

        public void Stop()
        {
            _timer?.Dispose();
            IsRunning = false;
        }

        private async Task CheckForUpdates()
        {
            try
            {
                var data = await EarthquakeService.FetchEarthquakeData();
                if (data == null) return;

                lock (_lock)
                {
                    if (EarthquakeStateStorage.IsNewEarthquake(data))
                    {
                        EarthquakeStateStorage.Save(data);
                        ShowNotification(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Polling Error: " + ex.ToString());
            }
        }

        private static bool IsNewEarthquake(EarthquakeInfo current, EarthquakeInfo previous)
        {
            return current.Datetime != previous.Datetime
                || current.Magnitude != previous.Magnitude
                || current.Wilayah != previous.Wilayah;
        }

        private void ShowNotification(EarthquakeInfo data)
        {
            var detail = $"Gempa dengan kekuatan {data.Magnitude} Magnitudo dan kedalaman {data.Kedalaman} km pada {data.Tanggal} - {data.Jam}. {data.Wilayah}";

            App.DispatcherQueue?.TryEnqueue(() =>
            {
                var notification = new AppNotificationBuilder()
                    .AddText("Informasi Gempa Terkini")
                    .AddText(detail)
                    .BuildNotification();

                AppNotificationManager.Default.Show(notification);
            });
        }
    }
}
