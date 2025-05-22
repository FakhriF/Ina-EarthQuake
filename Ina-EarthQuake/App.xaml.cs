using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using H.NotifyIcon;
using Ina_EarthQuake.Services;
using Ina_EarthQuake.Views;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AppNotifications;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ina_EarthQuake
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private readonly EarthquakePollingService _pollingService = new();
        public static DispatcherQueue? DispatcherQueue { get; private set; }
        public static bool HandleClosedEvents { get; set; } = true;
        public static Window? m_window;

        public App()
        {
            InitializeComponent();
            UnhandledException += App_UnhandledException;
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (!EnsureSingleInstance())
                return;

            InitializeMainWindow();
            InitializeNotifications();
            InitializeDispatcher();
            CheckForUpdates();
            InitializePollingService();
        }

        private static bool EnsureSingleInstance()
        {
            var keyInstance = AppInstance.FindOrRegisterInstanceForKey("main");
            if (!keyInstance.IsCurrentInstance)
            {
                keyInstance.RedirectActivationTo();
                Process.GetCurrentProcess().Kill();
                return false;
            }
            return true;
        }

        private void InitializeMainWindow()
        {
            m_window = new MainWindow();
            m_window.Closed += OnWindowClosed;
            m_window.Activate();
        }

        private void InitializeNotifications()
        {
            AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;
        }

        private static void InitializeDispatcher()
        {
            DispatcherQueue = m_window.DispatcherQueue;
        }

        private static void CheckForUpdates()
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(500); // Wait for UI to be ready
                await m_window.DispatcherQueue.EnqueueAsync(async () =>
                {
                    await UpdateChecker.CheckForUpdateAsync();
                });
            });
        }

        private void InitializePollingService()
        {
            if (IsNotificationsEnabled())
            {
                Debug.WriteLine("Notification set to ON | Pooling Service ON");
                _pollingService.Start();
            }
        }

        private static bool IsNotificationsEnabled()
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey("NotificationsEnabled") &&
                   (bool)ApplicationData.Current.LocalSettings.Values["NotificationsEnabled"];
        }

        private void OnWindowClosed(object sender, WindowEventArgs args)
        {
            if (HandleClosedEvents)
            {
                args.Handled = true;
                m_window.Hide();
            }
        }

        private void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            App.DispatcherQueue?.TryEnqueue(() =>
            {
                if (m_window is MainWindow mainWin)
                {
                    mainWin.BringToFront();
                }
            });
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            try
            {
                string logPath = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "crashlog.txt");
                File.WriteAllText(logPath, $"[CRASH] {e.Exception.Message}\n{e.Exception.StackTrace}");
            }
            catch { }

            e.Handled = true;
        }
    }

}
