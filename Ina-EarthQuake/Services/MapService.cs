using System.Collections.Generic;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui;
using Mapsui.UI.WinUI;
using Mapsui.Extensions;
using System.Linq;
using System.Diagnostics;
using Mapsui.Limiting;

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

        public void SetZoomLimits(MapControl mapControl)
        {
            mapControl.Map.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
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

        public void AddEarthquakeMarker(MapControl mapControl, double lat, double lon, double magnitude)
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
                Styles = CreateEarthquakeStyles(magnitude)
            };

            gempaLayer.Features = [.. gempaLayer.Features, earthquakeFeature];
            mapControl.Refresh();
        }

        private ICollection<IStyle> CreateEarthquakeStyles(double magnitude)
        {
            var color = magnitude < 4.0 ? new Color(255,255,0) :
                magnitude < 6.0 ? new Color(255, 165, 0) :
                new Color(255, 0, 0);

            var scale = 0.6 + (magnitude / 10.0);

            return new List<IStyle>
            {
                new SymbolStyle
                {
                    Fill = new Brush(new Color(color.R, color.G, color.B, 80)),
                    SymbolScale = scale * 2.0,
                    SymbolType = SymbolType.Ellipse
                },
                new SymbolStyle
                {
                    Fill = new Brush(new Color(color.R, color.G, color.B, 120)),
                    SymbolScale = scale * 1.5,
                    SymbolType = SymbolType.Ellipse
                },
                new SymbolStyle
                {
                    Fill = new Brush(color),
                    Outline = new Pen(Color.Black, 1),
                    SymbolScale = scale,
                    SymbolType = SymbolType.Ellipse
                }
            };
        }
    }
}