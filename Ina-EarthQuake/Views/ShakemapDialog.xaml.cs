using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShakemapDialog : ContentDialog
    {
        public ShakemapDialog(string shakemapCode, XamlRoot xamlRoot)
        {
            this.InitializeComponent();

            // Set XamlRoot agar dialog bisa muncul
            this.XamlRoot = xamlRoot;

            // Set gambar berdasarkan kode shakemap dari API
            string imageUrl = $"https://static.bmkg.go.id/{shakemapCode}";
            ShakemapImage.Source = new BitmapImage(new Uri(imageUrl));
        }
    }
}
