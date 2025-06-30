using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace GetStartedApp.Services
{
    public class ToastService : IToastService
    {
        private Window? _mainWindow;
        private WindowToastManager? _toastManager;

        public void Initialize(Window mainWindow)
        {
            _mainWindow = mainWindow;
            var topLevel=TopLevel.GetTopLevel(mainWindow);
            _toastManager = new WindowToastManager(topLevel) { MaxItems = 3 };
        }

        public void Show(string message)
        {
            _toastManager?.Show(new Toast(message));
        }
    }
}
