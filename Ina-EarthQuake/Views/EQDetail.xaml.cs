using Mapsui.Projections;
using Mapsui;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Mapsui.Tiling;
using Mapsui.Extensions;
using Ina_EarthQuake.Models;
using System.Diagnostics;
using Ina_EarthQuake.Services;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EQDetail : Page
    {
        public EQDetail()
        {
            this.InitializeComponent();

            MyMap.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is EarthquakeInfo eq)
            {
                Debug.WriteLine($"Navigated to detail: {eq.Tanggal} - Magnitude {eq.Magnitude}");
                this.DataContext = eq;
                UpdateUI(eq); // Pastikan eq tidak null sebelum dipakai
            }
            else
            {
                Debug.WriteLine("Data gempa tidak ditemukan atau tidak sesuai.");
            }
        }

        private Task UpdateUI(EarthquakeInfo eq)
        {
            if (eq != null)
            {
                UpdateEarthquakeDetails(eq);
            }
            else
            {
                EarthquakeDate.Text = "Error: Unable to fetch data.";
            }

            if (eq != null)
            {
                double lat = MapServices.ParseCoordinate(eq.Lintang);
                double longti = MapServices.ParseCoordinate(eq.Bujur, isLatitude: false);

                SetMapPosition(lat, longti);
                MapServices.AddEarthquakeMarker(lat, longti, MyMap);
            }

            return Task.CompletedTask;
        }

        private void UpdateEarthquakeDetails(EarthquakeInfo data)
        {
            EarthquakeDate.Text = data.Tanggal;
            EarthquakeHours.Text = data.Jam;
            EarthquakeMag.Text = data.Magnitude;
            EarthquakeDepth.Text = data.Kedalaman;
            EarthquakeRegion.Text = data.Wilayah;
            EarthquakeCoordinates.Text = $"{data.Lintang} {data.Bujur}";
            EarthquakeFelt.Text = data.Dirasakan;
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            MyMap.Map.Layers.Clear();
        }

        private void SetMapPosition(double lat, double longti)
        {
            var centerOfEarthquake = new MPoint(longti, lat);
            var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(centerOfEarthquake.X, centerOfEarthquake.Y).ToMPoint();

            MyMap.Map.Navigator.CenterOnAndZoomTo(sphericalMercatorCoordinate, MyMap.Map.Navigator.Resolutions[10]);

            MyMap.Map.Navigator.PanLock = true;
            MyMap.Map.Navigator.RotationLock = true;
        }
    }
}
