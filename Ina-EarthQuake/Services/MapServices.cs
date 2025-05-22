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
    public static class MapServices
    {
        public static double ParseCoordinate(string coordinate, bool isLatitude = true)
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
                    return result; // Longitude (Bujur) bisa positif atau negatif, sesuai dengan koordinat
                }
            }

            // Jika parsing gagal, kembalikan 0
            return 0;
        }

        public static void SetMapPosition(double lat, double longti, MapControl MyMap)
        {
            var centerOfEarthquake = new MPoint(longti, lat);
            var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(centerOfEarthquake.X, centerOfEarthquake.Y).ToMPoint();

            MyMap.Map.Navigator.CenterOnAndZoomTo(sphericalMercatorCoordinate, MyMap.Map.Navigator.Resolutions[8]);
            Debug.WriteLine("Zooooooom");

            //MyMap.Map.Navigator.PanLock = true;
            MyMap.Map.Navigator.RotationLock = true;
        }
        public static void AddEarthquakeMarker(double lat, double longti, MapControl MyMap)
        {
            var earthquakePosition = new MPoint(longti, lat);
            var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(earthquakePosition.X, earthquakePosition.Y).ToMPoint();

            if (MyMap.Map.Layers.FirstOrDefault(layer => layer.Name == "GempaLayer") is not MemoryLayer gempaLayer)
            {
                gempaLayer = new MemoryLayer
                {
                    Name = "GempaLayer",
                    Features = [],
                    Style = null
                };
                MyMap.Map.Layers.Add(gempaLayer);
            }

            // Buat titik gempa sebagai feature
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

            // Tambahkan ke layer
            gempaLayer.Features = [.. gempaLayer.Features, earthquakeFeature];
            MyMap.Refresh();
        }
    }
}