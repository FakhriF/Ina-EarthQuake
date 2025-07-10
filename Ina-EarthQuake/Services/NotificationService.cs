using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ina_EarthQuake.Models;

namespace Ina_EarthQuake.Services
{
    public class NotificationService : INotificationService
    {
        public void ShowNewEarthquakeNotification(EarthquakeInfo data)
        {
            var detail = $"Gempa dengan kekuatan {data.Magnitude} Magnitudo di kedalaman {data.Kedalaman} pada {data.Tanggal} - {data.Jam}. {data.Wilayah}";

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
