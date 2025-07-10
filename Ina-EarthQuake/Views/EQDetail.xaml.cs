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
using Ina_EarthQuake.ViewModels;
using System.ComponentModel;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EQDetail : Page
    {
        public EQDetailViewModel ViewModel { get; set; }

        public EQDetail()
        {
            this.InitializeComponent();

            ViewModel = new EQDetailViewModel();

            MyMap.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
            App.MapService.SetZoomLimits(MyMap);

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectedEarthquake))
            {
                DispatcherQueue.TryEnqueue(async () =>
                {
                    await Task.Delay(100);
                    UpdateMapPosition();
                });
            }
        }

        private void UpdateMapPosition()
        {
            var data = ViewModel.SelectedEarthquake;
            if (data == null) return;

            double lat = App.MapService.ParseCoordinate(data.Lintang);
            double lon = App.MapService.ParseCoordinate(data.Bujur, isLatitude: false);

            App.MapService.SetMapPosition(MyMap, lat, lon);
            App.MapService.AddEarthquakeMarker(MyMap, lat, lon, data.Magnitude.Value);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is EarthquakeInfo eq)
            {
                ViewModel.Initialize(eq);
            }
            else
            {
                Debug.WriteLine("[ERROR] Data gempa tidak ditemukan atau tidak sesuai.");
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            MyMap.Map.Layers.Clear();
        }
    }
}
