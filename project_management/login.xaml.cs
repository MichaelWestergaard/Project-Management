using project_management.Controllers;
using project_management.DAO;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications.Messages;

namespace project_management
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class Login : Window
    {

        Utilities utilities = new Utilities();

        public Login()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            new createUser().Show();
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {

            string email = EmailInput.Text.ToString();
            string password = PasswordInput.Password.ToString();
            
            if(email.Length != 0 && password.Length != 0)
            {
                User user = new UserDAO().GetUserByEmail(email);

                if(user != null)
                {
                    if (utilities.CheckPassword(password, user.Password))
                    {
                        MainController.Instance.User = user;
                        if (SaveLogin.IsChecked == true)
                        {
                            Properties.Settings.Default.Email = email;
                            Properties.Settings.Default.Password = password;
                            Properties.Settings.Default.AutoLogin = true;
                            Properties.Settings.Default.Save();
                        } else
                        {
                            Properties.Settings.Default.Email = "";
                            Properties.Settings.Default.Password = "";
                            Properties.Settings.Default.AutoLogin = false;
                            Properties.Settings.Default.Save();
                        }
                        new Home().Show();

                        utilities.GetNotifier().ShowSuccess("Velkommen, " + user.Firstname + "!");
                        this.Close();

                    } else
                    {
                        utilities.GetNotifier().ShowError("Email eller adgangskode er forkert");
                    }
                } else
                {
                    utilities.GetNotifier().ShowError("Email eller adgangskode er forkert");
                }
            } else
            {
                utilities.GetNotifier().ShowError("Husk og udfyld email og adgangskode!");

            }
        }
    }
}
