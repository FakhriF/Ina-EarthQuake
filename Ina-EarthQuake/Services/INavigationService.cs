using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ina_EarthQuake.Services
{
    public interface INavigationService
    {
        void NavigateTo(string pageKet, object? parameter = null);

        bool GoBack();
    }
}
