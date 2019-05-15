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
    /// Interaction logic for EditProject.xaml
    /// </summary>
    public partial class EditProject : Window
    {
        Project project;
        MainController mainController = MainController.Instance;
        private List<int> userList = new List<int>();
        List<User> projectUsers;
        ProjectDAO projectDAO = new ProjectDAO();
        
        InviteUserToProject inviteUserToProject;
        Utilities utilities = new Utilities();

        StackPanel projectList;

        public EditProject(int projectID, StackPanel projectList)
        {
            InitializeComponent();
            this.projectList = projectList;
            project = projectDAO.Read(projectID);

            if (project != null)
            {
                name.Text = project.Name;
                description.Text = project.Description;
                deadline.Text = project.DueDate.Date.ToString();

                projectUsers = projectDAO.GetProjectUsers(project.Id);

                foreach (User user in projectUsers)
                {
                    AddUserPicture(user);
                }
            }
            else
            {
                Close();
            }
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

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            string name = this.name.Text;
            string description = this.description.Text;
            DateTime deadline = this.deadline.SelectedDate.Value;

            if (!name.Equals(""))
            {
                project.Name = name;
                project.Description = description;
                project.DueDate = deadline;

                if (projectDAO.Update(project))
                {

                    foreach(User projectUser in projectUsers)
                    {
                        if (userList.Contains(projectUser.Id))
                        {
                            userList.Remove(projectUser.Id);
                        }
                    }

                    foreach (int userID in userList)
                    {
                        projectDAO.AddUserToProject(project.Id, userID);
                    }

                    if (inviteUserToProject != null)
                        inviteUserToProject.Close();

                    string content = "";

                    content += name[0];

                    if (name.Length > 1)
                        content += name[1];

                    foreach (UIElement element in projectList.Children)
                    {
                        if (element.Uid.Equals(project.Id.ToString()))
                        {
                            ((Button)element).Content = content.ToUpper();
                            break;
                        }
                    }

                    utilities.GetNotifier().ShowSuccess("Projektet blev opdateret!");
                    mainController.Dashboard.UpdatePage();
                    Close();
                }
            }
            else
            {
                utilities.GetNotifier().ShowError("Indtast venligst et projektnavn");
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

            ellipse.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri((user.Picture == "" || user.Picture == null) ? "https://pixelmator-pro.s3.amazonaws.com/community/avatar_empty@2x.png" : user.Picture))
            };

            MemberList.Children.Add(ellipse);
        }

        private void ButtonDeleteProject_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in projectList.Children)
            {
                if (element.Uid.Equals(project.Id.ToString()))
                {
                    projectList.Children.Remove(element);
                    break;
                }
            }

            if (mainController.Project.Id.Equals(project.Id))
            {
                if (projectDAO.Delete(project.Id))
                {
                    List<Project> projects = mainController.UserProjects();

                    if (projects.Count > 0)
                    {
                        mainController.Project = projects[0];
                    }
                    else
                    {
                        mainController.Project = null;
                    }

                    mainController.ChangeProject();

                    foreach (UIElement element in projectList.Children)
                    {
                        if (element.Uid.Equals(project.Id.ToString()))
                        {
                            projectList.Children.Remove(element);
                            break;
                        }
                    }
                    Close();
                }
            }
        }
    }
}
