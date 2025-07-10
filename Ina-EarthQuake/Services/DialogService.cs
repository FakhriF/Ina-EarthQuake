using Ina_EarthQuake.Views;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ina_EarthQuake.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> ShowConfirmationDialogAsync(string title, string message, string primaryButtonText = "OK", string secondaryButtonText = "Cancel")
        {
            if (App.m_window?.Content.XamlRoot == null) return false;

            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText,
                XamlRoot = App.m_window.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task ShowInfoDialogAsync(string title, string content, string closeButtonText = "OK")
        {
            if (App.m_window?.Content.XamlRoot == null) return;

            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = closeButtonText,
                XamlRoot = App.m_window.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        public async Task ShowShakemapDialogAsync(string shakemapCode)
        {
            if (App.m_window?.Content.XamlRoot == null) return;

            var dialog = new ShakemapDialog(shakemapCode)
            {
                XamlRoot = App.m_window.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
