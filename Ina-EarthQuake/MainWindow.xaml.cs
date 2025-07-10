using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

            MainFrame.Navigate(typeof(Views.HomePage));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Nav.IsBackEnabled = MainFrame.CanGoBack;

            UpdateSelectedNavItem(e.SourcePageType);
        }

        private void UpdateSelectedNavItem(Type currentPageType)
        {
            var selectedItem = Nav.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Tag is string tagString && Type.GetType(tagString) == currentPageType);

            //await Task.Delay(50);

            Nav.SelectedItem = selectedItem;
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
            if (args.IsSettingsInvoked)
            {
                MainFrame.Navigate(typeof(SettingsPage));
                return;
            }
            
            if (args.InvokedItemContainer?.Tag is string pageTag)
            {
                var pageType = Type.GetType(pageTag);

                if (pageType != null)
                {
                    MainFrame.Navigate(pageType);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[ERROR] Tipe halaman '{pageTag}' tidak ditemukan.");
                }
            }
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
