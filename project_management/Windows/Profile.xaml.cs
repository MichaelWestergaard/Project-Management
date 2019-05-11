using project_management.Controllers;
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

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        Home home;
        User user;
        public Profile(Home home)
        {
            user = MainController.Instance.User;

            InitializeComponent();
            UserName.Text = user.Firstname + " " + user.Lastname;
            this.home = home;
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditUser editUser = new EditUser();
            editUser.Show();
            Close();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Email = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.AutoLogin = false;
            Properties.Settings.Default.Save();
            home.Close();
            new Login().Show();
            Close();
        }
    }
}
