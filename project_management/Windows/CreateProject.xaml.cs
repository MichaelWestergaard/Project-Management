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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications.Messages;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window
    {
        Home home;
        MainController mainController = MainController.Instance;
        private List<int> userList = new List<int>();

        User user;
        InviteUserToProject inviteUserToProject;
        Utilities utilities = new Utilities();

        public CreateProject(Home home)
        {
            this.home = home;
            user = mainController.User;
            InitializeComponent();
            
            AddInvitedUser(user.Id);

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

            ellipse.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri((user.Picture == "" || user.Picture == null) ? "https://pixelmator-pro.s3.amazonaws.com/community/avatar_empty@2x.png" : user.Picture))
            };

            this.MemberList.Children.Add(ellipse);

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
            inviteUserToProject = new InviteUserToProject(this);
            inviteUserToProject.Show();
        }

        public void AddInvitedUser(int userID)
        {
            userList.Add(userID);
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            string name = this.name.Text;
            string description = this.description.Text;
            DateTime deadline = this.deadline.SelectedDate.Value;

            if (!name.Equals(""))
            {
                Project project = new Project();
                project.Name = name;
                project.Description = description;
                project.DueDate = deadline;
                project.ProjectOwnerID = user.Id;

                ProjectDAO projectDAO = new ProjectDAO();

                int projectID = projectDAO.CreateProject(project);
                
                foreach (int userID in userList)
                {
                    projectDAO.AddUserToProject(projectID, userID);
                }
                this.Close();
                
                if(inviteUserToProject != null)
                    inviteUserToProject.Close();

                utilities.GetNotifier().ShowSuccess("Projektet blev oprettet!");
                home.NewProjectElement(projectID, name);

                if (mainController.Project == null)
                {
                    mainController.Project = projectDAO.Read(projectID);
                    mainController.ChangeProject();
                }

            } else
            {
                utilities.GetNotifier().ShowError("Indtast venligst et projektnavn");
            }
        }
    }

    
}
