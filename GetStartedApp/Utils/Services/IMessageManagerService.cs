using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Utils.Services
{
    public interface IMessageManagerService
    {
        void Show(string message);

        void Initialize(Window mainWindow);
        void Show(string message, NotificationType type);
    }
}

