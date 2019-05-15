using project_management.Controllers;
using project_management.DAO;
using project_management.DTO;
using project_management.Windows;
using System;
using System.Windows;
using ToastNotifications.Messages;

namespace project_management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Utilities utilities = new Utilities();

        public MainWindow()
        {
            InitializeComponent();
            if(MainController.Instance.User != null)
            {
                new Home().Show();
            } else
            {
                if (Properties.Settings.Default.AutoLogin == true)
                {
                    string email = Properties.Settings.Default.Email;
                    string password = Properties.Settings.Default.Password;

                    if (!email.Equals("") && !password.Equals(""))
                    {
                        User user = new UserDAO().GetUserByEmail(email);

                        if (user != null)
                        {
                            if (utilities.CheckPassword(password, user.Password))
                            {
                                MainController.Instance.User = user;
                                new Home().Show();
                                this.Close();
                            } else
                            {
                                new Login().Show();
                                this.Close();
                            }
                        }
                    }
                } else
                {
                    new Login().Show();
                }
            }
            this.Close();
        }
    }
}
