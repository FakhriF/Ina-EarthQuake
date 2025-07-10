using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ina_EarthQuake.Services
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmationDialogAsync(string title, string message, string primaryButtonText = "OK", string secondaryButtonText = "Cancel");

        Task ShowInfoDialogAsync(string title, string content, string closeButtonText = "OK");

        Task ShowShakemapDialogAsync(string shakemapCode);

    }
}
