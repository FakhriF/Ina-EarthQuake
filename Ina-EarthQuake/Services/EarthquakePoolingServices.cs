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

    public class EarthquakePollingService
    {
        private Timer? _timer;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        private readonly EarthquakeService _earthquakeService;
        private readonly INotificationService _notificationService;

        public bool IsRunning { get; private set; } = false;

        public EarthquakePollingService(EarthquakeService earthquakeService, INotificationService notificationService)
        {
            _earthquakeService = earthquakeService;
            _notificationService = notificationService;
        }

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
                var data = await _earthquakeService.FetchLatestEarthquakeAsync();
                if (data == null) return;

               
                if (EarthquakeStateStorage.IsNewEarthquake(data))
                {
                    EarthquakeStateStorage.Save(data);
                    _notificationService.ShowNewEarthquakeNotification(data);
                 }
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ERROR] Polling Error: " + ex.ToString());
            }
        }

        private static bool IsNewEarthquake(EarthquakeInfo current, EarthquakeInfo previous)
        {
            return current.DateTime != previous.DateTime
                || current.Magnitude != previous.Magnitude
                || current.Wilayah != previous.Wilayah;
        }
    }
}
