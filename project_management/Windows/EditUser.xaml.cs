using project_management.Controllers;
using project_management.DAO;
using project_management.DTO;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ToastNotifications.Messages;

namespace project_management
{

    public partial class EditUser : Window
    {
        Utilities utilities = new Utilities();

        private User user;
        string filename, filePath;

        public EditUser()
        {
            user = MainController.Instance.User;
            InitializeComponent();
            firstName.Text = user.Firstname;
            lastName.Text = user.Lastname;
            email.Text = user.Email;
        }

        private void DelPicClick(object sender, RoutedEventArgs e)
        {
            this.imageBox.Visibility = Visibility.Collapsed;
            this.delPic.Visibility = Visibility.Collapsed;
            imageBox.Source = null;

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PictureUploadClick(object sender, RoutedEventArgs e)
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

        private bool InputsReq()
        {
            string firstName = this.firstName.Text;
            string lastName = this.lastName.Text;
            string password = this.password.Password;
            string repassword = this.repassword.Password;
            if (password == "")
            {
                user.Password = user.Password;
                repassword = user.Password;
                password = user.Password;
                user.Picture = filename == null ? user.Picture : filePath;
            }
            string email = this.email.Text;

            if (NameFieldReq(firstName) && NameFieldReq(lastName) && NameFieldReq(password) && NameFieldReq(repassword) && EmailReq(email))
            {
                if (PasswordReq(password) && password.Equals(repassword))
                {
                    return true;
                }
            }
            return false;
        }

      

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            string firstName = this.firstName.Text;
            string lastName = this.lastName.Text;
            string password = this.password.Password;
            string repassword = this.repassword.Password;
            string email = this.email.Text;

            user.Firstname = firstName;
            user.Lastname = lastName;
            user.Email = email;
            user.Picture = filename == null ? user.Picture : filePath;


            if (password == "")
            {
                user.Password = user.Password;
                repassword = user.Password;
                password = user.Password;
            }
            else
            {
                user.Password = utilities.EncryptPassword(password);

                if (Properties.Settings.Default.AutoLogin)
                {
                    Properties.Settings.Default.Email = user.Email;
                    Properties.Settings.Default.Password = user.Password;
                    Properties.Settings.Default.Save();
                }
            }

            if (InputsReq())
            {
                UserDAO userDAO = new UserDAO();
                userDAO.Update(user);
                this.Close();
            }
            else
            {
                if (!NameFieldReq(firstName))
                {
                    utilities.GetNotifier().ShowError("Navnfeltet må ikke være tomt");
                }
                else if (!NameFieldReq(lastName))
                {
                    utilities.GetNotifier().ShowError("Efternavnfeltet må ikke være tomt");
                }
                else if (!NameFieldReq(email))
                {
                    utilities.GetNotifier().ShowError("Emailfeltet må ikke være tomt");
                }
                else if (!EmailReq(email))
                {
                    utilities.GetNotifier().ShowError("E-mailadressen er ugyldig");
                }
                else if (!PasswordReq(password))
                {
                    utilities.GetNotifier().ShowError("Adgangskode lever ikke op til kravene. Prøv igen");
                }
                else if (!(password.Equals(repassword)))
                {
                    utilities.GetNotifier().ShowError("Koderne er ikke ens");
                }

            }

        }

        private bool NameFieldReq(string field)
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

        private bool EmailReq(string email)
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

        private bool PasswordReq(string password)
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
