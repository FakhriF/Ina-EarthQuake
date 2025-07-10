using Ina_EarthQuake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ina_EarthQuake.Services
{
    public interface INotificationService
    {
        void ShowNewEarthquakeNotification(EarthquakeInfo data);
    }
}
