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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications.Messages;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for InviteUserToProject.xaml
    /// </summary>
    public partial class InviteUserToProject : Window
    {
        UserDAO userDAO;
        ProjectDAO projectDAO;
        Object openedBy;
        Utilities utilities = new Utilities();

        public InviteUserToProject(Object openedBy)
        {
            this.openedBy = openedBy;
            InitializeComponent();
            userDAO = new UserDAO();
            projectDAO = new ProjectDAO();
        }
        
        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Member_Click(object sender, RoutedEventArgs e)
        {
            string email = this.email.Text;

            if (Utilities.IsValidEmail(email))
            {
                if (!userDAO.IsEmailFree(email))
                {
                    User user = userDAO.GetUserByEmail(email);
                    if(user != null)
                    {
                        if (openedBy is CreateProject)
                            ((CreateProject)openedBy).AddInvitedUser(user.Id);
                        AddUserPicture(user);
                    } else
                    {
                        utilities.GetNotifier().ShowError("Kunne ikke finde bruger, prøv venligst igen.");
                    }
                } else
                {
                    utilities.GetNotifier().ShowError("Der findes ikke en bruger med denne email adresse");
                }
            } else
            {
                utilities.GetNotifier().ShowError("Du bedes venligst indtaste en korrekt email adresse!");
            }
        }

        private void AddUserPicture(User user)
        {
            Ellipse ellipse = new Ellipse
            {
                Height = 30,
                Width = 30,
                Margin = new Thickness(5),
                ToolTip = user.Firstname + " " + user.Lastname
            };
            
            ellipse.Effect = new DropShadowEffect
            {
                Color = (Color)ColorConverter.ConvertFromString("#FFBBBBBB"),
                BlurRadius = 6,
                ShadowDepth = 1
            };
            Console.WriteLine("pic "+user.Picture);
            ellipse.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri((user.Picture == "" || user.Picture == null) ? "https://pixelmator-pro.s3.amazonaws.com/community/avatar_empty@2x.png" : user.Picture))
            };

            this.MemberList.Children.Add(ellipse);
            if (openedBy is CreateProject)
            {
                Ellipse ellipse2 = new Ellipse
                {
                    Height = 30,
                    Width = 30,
                    Margin = new Thickness(5),
                    ToolTip = user.Firstname + " " + user.Lastname
                };

                ellipse2.Effect = new DropShadowEffect
                {
                    Color = (Color)ColorConverter.ConvertFromString("#FFBBBBBB"),
                    BlurRadius = 6,
                    ShadowDepth = 1
                };

                ellipse2.Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("https://pixelmator-pro.s3.amazonaws.com/community/avatar_empty@2x.png"))
                };

                ((CreateProject)openedBy).MemberList.Children.Add(ellipse2);
            }
        }
    }
}
