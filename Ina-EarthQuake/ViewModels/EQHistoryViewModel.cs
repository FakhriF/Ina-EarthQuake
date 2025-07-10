using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ina_EarthQuake.Models;
using Ina_EarthQuake.Services;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ina_EarthQuake.ViewModels
{
    public partial class EQHistoryViewModel : ObservableObject
    {
        private readonly EarthquakeService _earthquakeService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<EarthquakeInfo> _earthquakeList = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ContentVisibility))]
        [NotifyPropertyChangedFor(nameof(LoadingVisibility))]
        private bool _isLoading = false;

        public Visibility ContentVisibility => IsLoading ? Visibility.Collapsed : Visibility.Visible;
        public Visibility LoadingVisibility => IsLoading ? Visibility.Visible : Visibility.Collapsed;

        public EQHistoryViewModel(INavigationService navigationService)
        {
            _earthquakeService = App.EarthquakeService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            EarthquakeList.Clear();

            var earthquakeData = await _earthquakeService.FetchEarthquakeHistoryAsync();
            if (earthquakeData != null)
            {
                foreach (var item in earthquakeData)
                {
                    EarthquakeList.Add(item);
                }
            }
            else
            {
                Debug.WriteLine("[ERROR] Tidak ada data gempa untuk ditampilkan.");
            }

            IsLoading = false;
        }

        [RelayCommand]
        private void GoToDetails(EarthquakeInfo? selectedItem)
        {
            if (selectedItem != null)
            {
                Debug.WriteLine($"Navigating to details for: {selectedItem.Tanggal} - Magnitude {selectedItem.Magnitude}");

                _navigationService.NavigateTo("EQDetail", selectedItem);
            }
        }

    }
}
