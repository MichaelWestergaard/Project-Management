using System;
using System.Text.RegularExpressions;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using System.Windows;
using System.Windows.Media;
using System.Security.Cryptography;

namespace project_management
{
    class Utilities
    {

        private RNGCryptoServiceProvider rNG = new RNGCryptoServiceProvider();

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

        public string EncryptPassword(string password)
        {
            byte[] salt = new byte[12];
            rNG.GetBytes(salt);

            byte[] hash = new Rfc2898DeriveBytes(password, salt, 100).GetBytes(20);

            byte[] hashByteArray = new byte[32];

            Array.Copy(salt, 0, hashByteArray, 0, 12);
            Array.Copy(hash, 0, hashByteArray, 12, 20);

            return Convert.ToBase64String(hashByteArray);
        }

        public bool CheckPassword(string input, string hashString)
        {
            byte[] hashArrayBytes = Convert.FromBase64String(hashString);
            
            byte[] salt = new byte[12];
            byte[] storedHash = new byte[20];

            Array.Copy(hashArrayBytes, 0, salt, 0, 12);
            Array.Copy(hashArrayBytes, 12, storedHash, 0, 20);

            byte[] hash = new Rfc2898DeriveBytes(input, salt, 100).GetBytes(20);

            string newHash = Convert.ToBase64String(hash);
            string storedHashS = Convert.ToBase64String(storedHash);

            if (newHash.Equals(storedHashS))
                return true;

            return false;
        }
    }
}
