using Mapsui.UI.WinUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ina_EarthQuake.Services
{
    public interface IMapService
    {
        double ParseCoordinate(string coordinate, bool isLatitude = true);
        void SetMapPosition(MapControl mapControl, double lat, double lon);
        void AddEarthquakeMarker(MapControl mapControl, double lat, double lon);
    }
}
