using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Ina_EarthQuake.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Ina_EarthQuake.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EQHistoryPage : Page
    {
        public ObservableCollection<EarthquakeInfo> EarthquakeList { get; } = [];

        public EQHistoryPage()
        {
            this.InitializeComponent();
            this.Loaded += async (s, e) => await UpdateData();
        }

        private async Task UpdateData()
        {
            var earthquakeData = await EarthquakeService.FetchEarthquakeHistory();
            if (earthquakeData != null)
            {
                EarthquakeList.Clear();
                foreach (var item in earthquakeData)
                {
                    EarthquakeList.Add(item);
                }
            }
            else
            {
                Debug.WriteLine("Tidak ada data gempa.");
            }
        }

        private void EQItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is EarthquakeInfo selectedItem)
            {
                Debug.WriteLine($"Selected Earthquake: {selectedItem.Tanggal} - Magnitude {selectedItem.Magnitude}");
                Frame.Navigate(typeof(EQDetail), selectedItem);
                //((MainWindow)Window.).MainFrame.Navigate(typeof(EQDetail), selectedItem);
            }
        }

    }
}
