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

    public partial class CreateUser : Window
    {
        Utilities utilities = new Utilities();

        string filename = "", filePath;
            
        public CreateUser()
        {
            InitializeComponent();
        }

        private void DelPicClick(object sender, RoutedEventArgs e)
        {
            this.imageBox.Visibility = Visibility.Collapsed;
            this.delPic.Visibility = Visibility.Collapsed;
            imageBox.Source = null;

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PictureUploadClick(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception exception)
            {
                utilities.GetNotifier().ShowError(utilities.HandleException(exception));
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

        private bool InputsReq()
        {
            string firstName = this.firstName.Text;
            string lastName = this.lastName.Text;
            string password = this.password.Password;
            string repassword = this.repassword.Password;
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

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            string firstName = this.firstName.Text;
            string lastName = this.lastName.Text;
            string password = this.password.Password;
            string repassword = this.repassword.Password;
            string email = this.email.Text;
            try
            {
                
                if (InputsReq())
                {

                    UserDAO userDAO = new UserDAO();
                    if (userDAO.IsEmailFree(email))
                    {
                        User user = new User();
                        user.Firstname = firstName;
                        user.Lastname = lastName;
                        user.Email = email;
                        user.Password = new Utilities().EncryptPassword(password);
                        user.Picture = filename.Equals("") ? "https://pixelmator-pro.s3.amazonaws.com/community/avatar_empty@2x.png" : filePath;

                        int userID = userDAO.CreateUser(user);

                        new Login().Show();
                        this.Close();
                    } else
                    {
                        utilities.GetNotifier().ShowError("Email adressen er allerede i brug!");
                    }
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
                    else if (!NameFieldReq(password) || !NameFieldReq(repassword))
                    {
                        utilities.GetNotifier().ShowError("Adgangskodefeltet må ikke være tomt");
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
            catch (Exception exception)
            {
                utilities.GetNotifier().ShowError(utilities.HandleException(exception));
            }
        }

        private bool NameFieldReq(string field)
        {
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
            var number = new Regex(@"[0-9]+");
            var uppercaseLetter = new Regex(@"[A-Z]+");
            var lowercaseLetter = new Regex(@"[a-z]+");
            var length = new Regex(@".{8,16}");

            if (!number.IsMatch(password) || !uppercaseLetter.IsMatch(password) || !lowercaseLetter.IsMatch(password) || !length.IsMatch(password))
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