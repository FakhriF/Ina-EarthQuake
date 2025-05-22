using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Mapsui.Tiling;
using System.Diagnostics;
using Ina_EarthQuake.Models;
using Ina_EarthQuake.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake.Views
{
    /// <summary>  
    /// An empty page that can be used on its own or navigated to within a Frame.  
    /// </summary>  
    /// 
    public sealed partial class HomePage : Page
    {
        readonly DispatcherTimer timer = new();

        private string shakemapCode = "";

        public HomePage()
        {
            this.InitializeComponent();
            MyMap.Map.Layers.Add(OpenStreetMap.CreateTileLayer());

            this.Loaded += async (s, e) => await UpdateUI();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += async (s, e) => await UpdateUI(); 
            timer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Debug.WriteLine("Sayonara!");
            MyMap.Map.Layers.Clear();
            timer?.Stop();  // Hentikan timer jika ada
        }

        private async Task UpdateUI()
        {
            var data = await EarthquakeService.FetchEarthquakeData();

            if (data != null)
            {
                UpdateEarthquakeDetails(data);
                ProcessEarthquakeData(data);
            }
            else
            {
                EarthquakeDate.Text = "Error: Unable to fetch data.";
            }

            UpdateLastUpdateInfo();
            shakemapCode = data?.Shakemap ?? "No Data";
            CheckShakemapAvailability();
        }

        private void UpdateEarthquakeDetails(EarthquakeInfo data)
        {
            EarthquakeDate.Text = data.Tanggal;
            EarthquakeHours.Text = data.Jam;
            EarthquakeMag.Text = data.Magnitude;
            EarthquakeDepth.Text = data.Kedalaman;
            EarthquakeRegion.Text = data.Wilayah;
            EarthquakeCoordinates.Text = $"{data.Lintang} {data.Bujur}";
            EarthquakePotential.Text = data.Potensi;
            EarthquakeFelt.Text = data.Dirasakan;
        }

        private void ProcessEarthquakeData(EarthquakeInfo data)
        {
            double lat = MapServices.ParseCoordinate(data.Lintang);
            double longti = MapServices.ParseCoordinate(data.Bujur, isLatitude: false);

            MapServices.SetMapPosition(lat, longti, MyMap);
            MapServices.AddEarthquakeMarker(lat, longti, MyMap);
        }

        private void UpdateLastUpdateInfo()
        {
            DateTime currentDateTime = DateTime.Now;
            LastUpdate.Text = $"Informasi Terakhir: {currentDateTime:dd/MM/yyyy HH:mm:ss}";
        }

        private void CheckShakemapAvailability()
        {
            bool available = shakemapCode != "No Data";
            ShakemapButton.IsEnabled = available;
        }


        private async void ShakeMap_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ShakemapDialog(shakemapCode, this.XamlRoot);
            await dialog.ShowAsync();

        }
    }
}


