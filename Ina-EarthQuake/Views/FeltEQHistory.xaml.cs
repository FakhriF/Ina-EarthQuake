using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Ina_EarthQuake.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Ina_EarthQuake.Services;
using Ina_EarthQuake.ViewModels;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeltEQHistory : Page
    {
        public EQHistoryViewModel ViewModel { get; }

        public FeltEQHistory()
        {
            this.InitializeComponent();

            var navigationService = new NavigationService();

            ViewModel = new EQHistoryViewModel(navigationService, "Felt");
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadDataCommand.ExecuteAsync(null);

        }

        //private void EQItem_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    if (sender is FrameworkElement fe && fe.Tag is EarthquakeInfo selectedItem)
        //    {
        //        Debug.WriteLine($"Selected Earthquake: {selectedItem.Tanggal} - Magnitude {selectedItem.Magnitude}");
        //        Frame.Navigate(typeof(EQDetail), selectedItem);
        //        //((MainWindow)Window.).MainFrame.Navigate(typeof(EQDetail), selectedItem);
        //    }
        //}

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is EarthquakeInfo selectedItem)
            {
                ViewModel.GoToDetailsCommand.Execute(selectedItem);
            }
        }
    }
}
