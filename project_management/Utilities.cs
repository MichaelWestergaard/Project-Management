using System;
using System.Text.RegularExpressions;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using System.Windows;
using System.Windows.Media;

namespace project_management
{
    class Utilities
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public Notifier GetNotifier()
        {
            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
            return notifier;
        }

        public Brush GetColor(string hex)
        {
            return (Brush)new BrushConverter().ConvertFrom(hex);
        }

        public bool CheckOpen(Type window)
        {
            bool isOpen = false;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].GetType() == window)
                {
                    isOpen = true;
                    Application.Current.Windows[i].Activate();
                    break;
                }
            }
            if(isOpen)
            {
                return true;
            } else
            {
                return false;
            }
        }

    }
}
