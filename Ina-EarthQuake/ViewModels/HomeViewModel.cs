using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ina_EarthQuake.Models;
using Ina_EarthQuake.Services;
using Microsoft.UI.Composition;
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
    public partial class HomeViewModel : ObservableObject
    {
        private readonly EarthquakeService _earthquakeService;
        private readonly IDialogService _dialogService;

        private readonly DispatcherTimer _timer;

        [ObservableProperty]
        private EarthquakeInfo? _latestEarthquake;

        [ObservableProperty]
        private string _lastUpdateText = "Memuat data....";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsShakemapAvailable))]
        private string _shakemapCode = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ContentVisibility))]
        [NotifyPropertyChangedFor(nameof(LoadingVisibility))]
        private bool _isLoading = true;

        public Visibility ContentVisibility => IsLoading ? Visibility.Collapsed : Visibility.Visible;
        public Visibility LoadingVisibility => IsLoading ? Visibility.Visible : Visibility.Collapsed;

        [ObservableProperty]
        private ObservableCollection<DetailItem> _earthquakeDetails = new();

        public bool IsShakemapAvailable => !string.IsNullOrEmpty(ShakemapCode) && ShakemapCode != "No Data";

        public HomeViewModel()
        {
            _earthquakeService = App.EarthquakeService;
            _dialogService = App.DialogService;
            
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            _timer.Tick += (s, e) => LoadDataCommand.Execute(null);
        }

        private void PopulateDetailList()
        {
            EarthquakeDetails.Clear();
            if (LatestEarthquake == null) return;

            EarthquakeDetails.Add(new DetailItem
                {
                    IconGlyph = "\uE707",
                    Title = "Titik Koordinat",
                    Value = $"{LatestEarthquake.Lintang}, {LatestEarthquake.Bujur}"
                });

            EarthquakeDetails.Add(new DetailItem
            {
                IconGlyph = "\uEB44",
                Title = "Dirasakan",
                Value = LatestEarthquake.Dirasakan
            });

            EarthquakeDetails.Add(new DetailItem
            {
                IconGlyph = "\uE7B7",
                Title = "Wilayah",
                Value = LatestEarthquake.Wilayah
            });

            EarthquakeDetails.Add(new DetailItem
            {
                IconGlyph = "\uE789",
                Title = "Potensi",
                Value = LatestEarthquake.Potensi
            });
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var data = await _earthquakeService.FetchLatestEarthquakeAsync();
                if (data != null)
                {
                    LatestEarthquake = data;
                    ShakemapCode = data.Shakemap;
                    LastUpdateText = $"Informasi Terakhir: {DateTime.Now:dd/MM/yyyy HH:mm:ss} WIB";

                    PopulateDetailList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Gagal memuat data gempa: {ex.Message}");
                LastUpdateText = "Gagal memuat data gempa. Silakan coba lagi.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ShowShakemapDialogAsync()
        {
            if (!IsShakemapAvailable || ShakemapCode == null) return;

            await _dialogService.ShowShakemapDialogAsync(ShakemapCode);
        }

        public void StartTimer() => _timer.Start();
        public void StopTimer() => _timer.Stop();
    }
}
