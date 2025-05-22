using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Ina_EarthQuake.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            SetupTitleBar();
            SetupWindowSize();
            SetupNavigation();

        }

        private void SetupTitleBar()
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            //AppWindow.SetIcon("Assets/AppIcon.ico");
            AppWindow.SetTaskbarIcon("Assets/AppIcon.ico");
            AppWindow.SetTitleBarIcon("Assets/AppIcon.ico");
        }

        private void SetupWindowSize()
        {
            OverlappedPresenter presenter = OverlappedPresenter.Create();

            presenter.PreferredMinimumWidth = 1000;
            presenter.PreferredMinimumHeight = 400;

            AppWindow.SetPresenter(presenter);
        }

        private void SetupNavigation()
        {
            Nav.ItemInvoked += Nav_ItemInvoked;
            Nav.BackRequested += Nav_BackRequested;

            MainFrame.Navigated += MainFrame_Navigated;

            var homeItem = Nav.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => (string)item.Content == "Beranda");

            if (homeItem != null)
            {
                Nav.SelectedItem = homeItem;
                MainFrame.Navigate(typeof(HomePage));
            }

            Nav.IsBackEnabled = MainFrame.CanGoBack;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Aktifkan tombol back hanya jika sedang di halaman detail
            if (e.SourcePageType == typeof(EQDetail))
            {
                Nav.IsBackEnabled = true;
            }
            else
            {
                Nav.IsBackEnabled = false;
            }

            // Optional: update selected nav item agar tetap sinkron
            UpdateSelectedNavItem(e.SourcePageType);
        }

        private void UpdateSelectedNavItem(Type currentPageType)
        {
            if (currentPageType == typeof(HomePage))
                Nav.SelectedItem = Nav.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(i => (string)i.Content == "Beranda");
            else if (currentPageType == typeof(EQHistoryPage))
                Nav.SelectedItem = Nav.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(i => (string)i.Content == "Riwayat Gempa");
            else if (currentPageType == typeof(SafetyInfoPage))
                Nav.SelectedItem = Nav.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(i => (string)i.Content == "Informasi Keselamatan");
            else if (currentPageType == typeof(GlossaryPage))
                Nav.SelectedItem = Nav.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(i => (string)i.Content == "Glosarium");
            else
                Nav.SelectedItem = null; // Untuk subpage (misalnya EQDetail)
        }




        private void Nav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void Nav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem is string selectedItem)
            {
                switch (selectedItem)
                {
                    case "Beranda":
                        MainFrame.Navigate(typeof(HomePage));
                        break;
                    case "Riwayat Gempa":
                        MainFrame.Navigate(typeof(EQHistoryPage));
                        break;
                    case "Informasi Keselamatan":
                        MainFrame.Navigate(typeof(SafetyInfoPage));
                        break;
                    case "Glosarium":
                        MainFrame.Navigate(typeof(GlossaryPage));
                        break;
                    case "Settings":
                        MainFrame.Navigate(typeof(SettingsPage));
                        break;
                }
            }
            GC.Collect();
        }

        public void BringToFront()
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            ShowWindow(hwnd, SW_RESTORE);
            SetForegroundWindow(hwnd);
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

    }
}
