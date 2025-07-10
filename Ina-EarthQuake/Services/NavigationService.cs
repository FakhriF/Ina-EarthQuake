using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ina_EarthQuake.Services
{
    public class NavigationService : INavigationService
    {
        private static Frame? AppFrame => (App.m_window as MainWindow)?.MainFrame;

        private readonly Dictionary<string, Type> _pages = new();

        public NavigationService()
        {
            Configure("EQDetail", typeof(Views.EQDetail));
        }

        private void Configure(string key, Type pageType)
        {
            lock (_pages)
            {
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"[EXCEPTION] Kunci halaman '{key}' sudah terdaftar.");
                }
                _pages.Add(key, pageType);
            }
        }

        public void NavigateTo(string pageKet, object? parameter = null)
        {
            if (AppFrame == null) return;

            lock (_pages)
            {
                if (!_pages.TryGetValue(pageKet, out var pageType))
                {
                    throw new ArgumentException($"[EXCEPTION] Halaman '{pageKet}' tidak ditemukan.");
                }

                AppFrame.Navigate(pageType, parameter);
            }
        }

        public bool GoBack()
        {
            if (AppFrame == null || !AppFrame.CanGoBack) return false;
            AppFrame.GoBack();
            return true;
        }
    }
}
