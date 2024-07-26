using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestingAssistant.Services
{
    public interface INotificationService
    {
        void ShowNotification(string message, string title = "通知", NotificationType type = NotificationType.Information,
            NotificationPosition position = NotificationPosition.BottomRight, int duration = 3000);
    }

    public class NotificationService : INotificationService
    {
        private readonly TopLevel _top;

        private readonly WindowNotificationManager _manager;

        public NotificationService(TopLevel top)
        {
            _top = top;
            _manager = new WindowNotificationManager(top);
        }

        public void ShowNotification(string message, string title = "通知", NotificationType type = NotificationType.Information, NotificationPosition position = NotificationPosition.BottomRight, int duration = 3000)
        {
            _manager.Position = position;
            _manager.Show(new Notification(title, message, type, TimeSpan.FromMilliseconds(duration)));
        }
    }
}