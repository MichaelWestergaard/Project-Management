using Microsoft.Win32;
using project_management.DAO;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using ToastNotifications.Messages.Error;

namespace project_management
{
    
    public partial class createUser : Window
    {
        Utilities utilities = new Utilities();

        string filename, filePath;
            
        public createUser()
        {
            InitializeComponent();
        }

        private void delPicClick(object sender, RoutedEventArgs e)
        {
            this.imageBox.Visibility = Visibility.Collapsed;
            this.delPic.Visibility = Visibility.Collapsed;
            imageBox.Source = null;

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void pictureUploadClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        "Portable Network Graphic (*.png)|*.png";
            if (dlg.ShowDialog() == true)
            {
                imageBox.Source = new BitmapImage(new Uri(dlg.FileName, UriKind.Absolute));
                this.imageBox.Visibility = Visibility.Visible;
                this.delPic.Visibility = Visibility.Visible;

                filename = string.Format(@"{0}.png", Guid.NewGuid());

                filePath = "https://projectmanagement.michaelwestergaard.dk/images/" + filename;

                WebClient client = new WebClient
                {
                    Credentials = new NetworkCredential("michaelwestergaa.dk", "tim")
                };
                client.UploadFile("ftp://michaelwestergaard.dk/projectmanagement/images/" + filename, dlg.FileName);
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private bool inputsReq()
        {
            string firstName = this.firstName.Text;
            string lastName = this.lastName.Text;
            string password = this.password.Password;
            string repassword = this.repassword.Password;
            string email = this.email.Text;

            if (nameFieldReq(firstName) && nameFieldReq(lastName) && nameFieldReq(password) && nameFieldReq(repassword) && emailReq(email))
            {
                if (passwordReq(password) && password.Equals(repassword))
                {
                    return true;
                }
            }
            return false;
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            string firstName = this.firstName.Text;
            string lastName = this.lastName.Text;
            string password = this.password.Password;
            string repassword = this.repassword.Password;
            string email = this.email.Text;

            inputsReq();

            if (inputsReq())
            {
                User user = new User();
                user.Firstname = firstName;
                user.Lastname = lastName;
                user.Email = email;
                user.Password = password;
                user.Picture = filename.Equals("") ? "https://pixelmator-pro.s3.amazonaws.com/community/avatar_empty@2x.png" : filePath;

                UserDAO userDAO = new UserDAO();
                int userID = userDAO.CreateUser(user);

                new Login().Show();
                this.Close();
            } else
            {
                if (!nameFieldReq(firstName))
                {
                    utilities.GetNotifier().ShowError("Navnfeltet må ikke være tomt");
                }
                else if (!nameFieldReq(lastName))
                {
                    utilities.GetNotifier().ShowError("Efternavnfeltet må ikke være tomt");
                }
                else if (!nameFieldReq(email))
                {
                    utilities.GetNotifier().ShowError("Emailfeltet må ikke være tomt");
                }
                else if (!emailReq(email))
                {
                    utilities.GetNotifier().ShowError("E-mailadressen er ugyldig");
                }
                else if (!nameFieldReq(password) || !nameFieldReq(repassword))
                {
                    utilities.GetNotifier().ShowError("Adgangskodefeltet må ikke være tomt");
                }
                else if (!passwordReq(password))
                {
                    utilities.GetNotifier().ShowError("Adgangskode lever ikke op til kravene. Prøv igen");
                }
                else if (!(password.Equals(repassword)))
                {
                    utilities.GetNotifier().ShowError("Koderne er ikke ens");
                }
            }
        }

        private bool nameFieldReq(string field)
        {
            //string lastName = this.lastName.Text;
            if (field == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool emailReq(string email)
        {
            try
            {
                var checker = new System.Net.Mail.MailAddress(email);
                return checker.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool passwordReq(string password)
        {
            var nummer = new Regex(@"[0-9]+");
            var stortBogstav = new Regex(@"[A-Z]+");
            var lilleBogstav = new Regex(@"[a-z]+");
            var antalChar = new Regex(@".{8,16}");

            if (!nummer.IsMatch(password) || !stortBogstav.IsMatch(password) || !lilleBogstav.IsMatch(password) || !antalChar.IsMatch(password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}