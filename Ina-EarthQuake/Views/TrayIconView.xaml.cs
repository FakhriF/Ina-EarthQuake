using Microsoft.UI.Xaml.Controls;
using H.NotifyIcon;
using System.Windows.Input;
using Ina_EarthQuake.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake.Views
{
    public sealed partial class TrayIconView : UserControl
    {
        private bool _isWindowVisible;

        public bool IsWindowVisible => _isWindowVisible;

        public ICommand ShowHideWindowCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        public TrayIconView()
        {
            InitializeComponent();

            ShowHideWindowCommand = new RelayCommand(ShowHideWindow);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
        }

        public void ShowHideWindow()
        {
            var window = App.m_window;
            if (window == null)
            {
                return;
            }

            if (window.Visible)
            {
                window.Hide();
            }
            else
            {
                window.Show();
            }

            _isWindowVisible = window.Visible;
        }

        public void ExitApplication()
        {
            App.HandleClosedEvents = false;
            TrayIcon.Dispose();
            App.m_window?.Close();
        }
    }

}
