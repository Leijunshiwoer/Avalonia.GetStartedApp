using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Services
{
    public interface IToastService
    {
        void Show(string message);

        void Initialize(Window mainWindow);
    }
}
