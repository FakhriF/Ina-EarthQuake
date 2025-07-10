using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Mapsui.Tiling;
using System.Diagnostics;
using Ina_EarthQuake.Models;
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
    /// 
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; }
        private readonly IMapService _mapService;

        public HomePage()
        {
            InitializeComponent();

            ViewModel = new HomeViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            MyMap.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        }

        private async void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.LatestEarthquake))
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
            if(ViewModel.LatestEarthquake != null)
            {
                var data = ViewModel.LatestEarthquake;
                double lat = App.MapService.ParseCoordinate(data.Lintang!);
                double lon = App.MapService.ParseCoordinate(data.Bujur!, isLatitude: false);

                App.MapService.SetMapPosition(MyMap, lat, lon);
                App.MapService.AddEarthquakeMarker(MyMap, lat, lon);
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadDataCommand.ExecuteAsync(null);

            ViewModel.StartTimer();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ViewModel.StopTimer();
        }
    }
}


