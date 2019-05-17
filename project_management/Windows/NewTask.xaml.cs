using project_management.Elements;
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
using project_management.DTO;
using project_management.DAO;
using Task = project_management.DTO.Task;
using System.Text.RegularExpressions;
using ToastNotifications;
using ToastNotifications.Messages;
using project_management.Controllers;
using Section = project_management.DTO.Section;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for NewTask.xaml
    /// </summary>
    
    public partial class NewTask : Window
    {
        private TaskDAO taskDAO = new TaskDAO();
        private StackPanel currentSection;
        private int sectionID;

        private int assignedUserID = 0;
        private UserAvatar assignedUserAvatar = null;
        Utilities utilities = new Utilities();
        Section section;

        public NewTask()
        {
            InitializeComponent();
        }

        public NewTask(StackPanel currentSection)
        {
            InitializeComponent();

            try
            {
                this.currentSection = currentSection;

                string sectionName = currentSection.Name;

                sectionID = int.Parse(sectionName.Remove(0, "Section".Length));
            
                StackPanel projectUsers = (StackPanel) FindName("ProjectUsers");
                section = new SectionDAO().Read(sectionID);

                List<User> users = new ProjectDAO().GetProjectUsers(section.ProjectId);
            
                foreach (User user in users)
                {
                    UserAvatar userAvatar = new UserAvatar(this, user.Id);

                    userAvatar.Uid = user.Id.ToString();
                    userAvatar.UserImage.ImageSource = new BitmapImage(new Uri(user.Picture));
                    userAvatar.ToolTip = user.Firstname + " " + user.Lastname;
                    projectUsers.Children.Add(userAvatar);
                }
            }
            catch (Exception exception)
            {
                utilities.GetNotifier().ShowError(utilities.HandleException(exception));
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

        public void AssignUser(UserAvatar userAvatar, int id)
        {
            if (assignedUserAvatar != null)
                ((Button)assignedUserAvatar.FindName("AssignMemberToTask")).BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF2196F3");

            if (assignedUserID != id)
            {
                ((Button)userAvatar.FindName("AssignMemberToTask")).BorderBrush = (Brush)new BrushConverter().ConvertFrom("#d32f2f");
                assignedUserAvatar = userAvatar;
                assignedUserID = id;
            }
            else
            {
                assignedUserID = 0;
                assignedUserAvatar = null;
            }
        }

        private bool ValidateInput()
        {
            if (title.Text != "")
            {
                if (description.Text != "")
                {
                    if (estimation.Text != "" && double.TryParse(estimation.Text, out double resultEstimation))
                    {
                        if (priority.Text != "" && int.TryParse(priority.Text, out int resultPriority))
                        {
                            if (deadline.Text != "")
                            {
                                DateTime result;
                                if (DateTime.TryParse(deadline.Text, out result))
                                {
                                    if(result >= DateTime.Today)
                                    {
                                        DateTime projectDeadLine = new ProjectDAO().Read(section.ProjectId).DueDate;
                                        if (result <= projectDeadLine)
                                        {
                                            return true;
                                        } else
                                        {
                                            new Utilities().GetNotifier().ShowError("Opgaven overskrider projektets deadline d. " + projectDeadLine);
                                        }
                                    }
                                    else
                                    {
                                        new Utilities().GetNotifier().ShowError("Du kan ikke vælge en deadline der allerede er overskredet");
                                    }
                                }
                                else
                                {
                                    new Utilities().GetNotifier().ShowError("Vælg venligst en korrekt dato");
                                }
                            }
                            else
                            {
                                new Utilities().GetNotifier().ShowError("Udfyld deadline dato");
                            }
                        }
                        else
                        {
                            new Utilities().GetNotifier().ShowError("Udfyld prioritet");
                        }
                    }
                    else
                    {
                        new Utilities().GetNotifier().ShowError("Udfyld estimation");
                    }
                }
                else
                {
                    new Utilities().GetNotifier().ShowError("Udfyld beskrivelse");
                }
            }
            else
            {
                new Utilities().GetNotifier().ShowError("Udfyld navn");
            }

            return false;
        }

        private void ButtnCreateTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    string taskName = title.Text;
                    string taskDescription = description.Text;
                    double taskEstimation = Double.Parse(estimation.Text);
                    int taskPriority = int.Parse(priority.Text);
                    DateTime taskDeadline = DateTime.Parse(deadline.Text);

                    User assignedUser = assignedUserID != 0 ? new UserDAO().Read(assignedUserID) : null;

                    Task task = new Task(null, null, assignedUser, sectionID, taskName, taskDescription, taskEstimation, taskPriority, taskDeadline);

                    int taskID = taskDAO.CreateTask(task);

                    if (taskDAO.Read(taskID) != null)
                    {
                        TaskElement taskElement = new TaskElement(taskID);

                        taskElement.TaskID.Name = "Task" + taskID;

                        taskElement.title.Text = taskName;
                        taskElement.description.Text = taskDescription;

                        if (assignedUser != null)
                        {
                            taskElement.avatar.ImageSource = new BitmapImage(new Uri(assignedUser.Picture));
                            taskElement.UserButton.ToolTip = assignedUser.Firstname + " " + assignedUser.Lastname;
                        }

                        currentSection.Children.Insert(currentSection.Children.Count - 1, taskElement);
                        MainController.Instance.Dashboard.UpdatePage();
                        this.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                utilities.GetNotifier().ShowError(utilities.HandleException(exception));
            }
        }

        private void Priority_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
        
    }
}
