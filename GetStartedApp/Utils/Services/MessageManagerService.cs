using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace GetStartedApp.Utils.Services
{
    public class MessageManagerService : IMessageManagerService
    {
        private WindowToastManager? _toastManager;
        private WindowNotificationManager? _notificationManager;

        public void Initialize(Window mainWindow)
        {
            var topLevel=TopLevel.GetTopLevel(mainWindow);
            _toastManager = new WindowToastManager(topLevel) { MaxItems = 3 };
            _notificationManager = new WindowNotificationManager(topLevel) { MaxItems = 3 };
        }
        public void Initialize(UserControl control)
        {
            var topLevel = TopLevel.GetTopLevel(control);
            _toastManager = new WindowToastManager(topLevel) { MaxItems = 3 };
            _notificationManager = new WindowNotificationManager(topLevel) { MaxItems = 3 };
        }


        public void Show(string message)
        {
            _toastManager?.Show(new Toast(message));
        }

        public void Show(string message, NotificationType type)
        {
            _notificationManager?.Show(new Notification() { Content=message,Type=type}); 
        }

    }
}
