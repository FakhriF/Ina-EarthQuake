using System.Collections.Generic;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui;
using Mapsui.UI.WinUI;
using Mapsui.Extensions;
using System.Linq;
using System.Diagnostics;

namespace Ina_EarthQuake.Services
{
    public class MapService : IMapService
    {
        public double ParseCoordinate(string coordinate, bool isLatitude = true)
        {
            string cleanedCoordinate = coordinate;

            // Menghapus label untuk Lintang (LU/LS) atau Bujur (BT)
            if (isLatitude)
            {
                cleanedCoordinate = cleanedCoordinate.Replace(" LU", "").Replace(" LS", "");
            }
            else
            {
                cleanedCoordinate = cleanedCoordinate.Replace(" BT", "");
            }

            // Mencoba untuk mem-parsing string yang sudah dibersihkan
            if (double.TryParse(cleanedCoordinate, out double result))
            {
                // Jika ini adalah latitude (Lintang)
                if (isLatitude)
                {
                    return coordinate.EndsWith("LS") ? -result : result; // Jika "LS", berarti negatif
                }
                else
                {
                    return result;
                }
            }

            return 0;
        }

        public void SetMapPosition(MapControl mapControl, double lat, double lon)
        {
            var centerOfEarthquake = new MPoint(lon, lat);
            var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(centerOfEarthquake.X, centerOfEarthquake.Y).ToMPoint();

            mapControl.Map.Navigator.CenterOnAndZoomTo(sphericalMercatorCoordinate, mapControl.Map.Navigator.Resolutions[9]);

            //MyMap.Map.Navigator.PanLock = true;
            mapControl.Map.Navigator.RotationLock = true;

            Debug.WriteLine("[MAPS] Berhasil Pindah dan Zoom Sesuai Titik");
        }

        public void AddEarthquakeMarker(MapControl mapControl, double lat, double lon)
        {
            var earthquakePosition = new MPoint(lon, lat);
            var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(earthquakePosition.X, earthquakePosition.Y).ToMPoint();

            if (mapControl.Map.Layers.FirstOrDefault(layer => layer.Name == "GempaLayer") is not MemoryLayer gempaLayer)
            {
                gempaLayer = new MemoryLayer
                {
                    Name = "GempaLayer",
                    Features = [],
                    Style = null
                };
                mapControl.Map.Layers.Add(gempaLayer);
            }

            var earthquakeFeature = new PointFeature(sphericalMercatorCoordinate)
            {
                Styles =
                [
                    new SymbolStyle
                    {
                        Fill = new Brush { Color = Color.Red },
                        Outline = new Pen { Color = Color.Black, Width = 1 },
                        SymbolScale = 0.75,
                        SymbolType = SymbolType.Ellipse
                    }
                ]
            };

            gempaLayer.Features = [.. gempaLayer.Features, earthquakeFeature];
            mapControl.Refresh();
        }
    }
}