using CommunityToolkit.Mvvm.ComponentModel;
using Ina_EarthQuake.Models;
using Ina_EarthQuake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ina_EarthQuake.ViewModels
{
    public partial class EQDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Coordinates))]
        private EarthquakeInfo? _selectedEarthquake;

        public string Coordinates => $"{SelectedEarthquake?.Lintang} {SelectedEarthquake?.Bujur}";

        public IMapService MapService { get; set; }

        public EQDetailViewModel()
        {
            MapService = App.MapService;
        }

        public void Initialize(EarthquakeInfo earthquake)
        {
            SelectedEarthquake = earthquake;
        }
    }
}
