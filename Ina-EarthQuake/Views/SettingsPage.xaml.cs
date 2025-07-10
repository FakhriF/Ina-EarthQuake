using Ina_EarthQuake.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        //private readonly EarthquakePollingService _pollingService = new();

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            
            //if (StartupHelper.IsAutoStartEnabled("Ina-EarthQuake"))
            //{
            //    AutostartToggle.IsOn = true;
            //}

            if (ApplicationData.Current.LocalSettings.Values.TryGetValue("NotificationsEnabled", out object? value))
            {
                NotificationToggle.IsOn = (bool)value;
            }
        }


        private void NotificationToggle_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    Debug.WriteLine("[ACTION] Notification Switch is ON");
                    //_pollingService.Start();
                    ApplicationData.Current.LocalSettings.Values["NotificationsEnabled"] = true;
                }
                else
                {
                    Debug.WriteLine("[ACTION] Notification Switch is OFF");
                    //_pollingService.Stop();
                    ApplicationData.Current.LocalSettings.Values["NotificationsEnabled"] = false;
                }
            }
        }

        private void AutostartToggle_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    Debug.WriteLine("[ACTION] Autostart Switch is ON");
                    //StartupHelper.EnableAutoStart("Ina-EarthQuake");
                }
                else
                {
                    Debug.WriteLine("[ACTION] Autostart Switch is OFF");
                    //StartupHelper.DisableAutoStart("Ina-EarthQuake");
                }
            }
        }

        private void BackgroundTaskToggle_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    Debug.WriteLine("[ACTION] Background Task is ON");
                }
                else
                {
                    Debug.WriteLine("[ACTION] Background Task is OFF");
                }
            }
        }
    }
}
