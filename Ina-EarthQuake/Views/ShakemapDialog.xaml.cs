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
        public string ShakemapImageUrl { get; }

        public ShakemapDialog(string shakemapCode)
        {
            this.InitializeComponent();

            ShakemapImageUrl = $"https://data.bmkg.go.id/DataMKG/TEWS/{shakemapCode}";
        }
    }
}
