
using project_management.DAO;
using project_management.DTO;
using project_management.Elements;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ToastNotifications.Messages;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {

        private TaskDAO taskDAO = new TaskDAO();
        private TaskElement taskElement;
        private Task task;

        private int assignedUserID = 0;
        private UserAvatar assignedUserAvatar = null;
        
        public EditTask(TaskElement taskElement)
        {
            this.taskElement = taskElement;
            task = taskDAO.Read(taskElement.taskID);
            InitializeComponent();

            title.Text = task.Name;
            description.Text = task.Description;
            deadline.Text = task.DueDate.ToString("dd-MM-yyyy");
            estimation.Text = task.EstimatedTime.ToString();
            priority.Text = task.Priority.ToString();


            StackPanel projectUsers = (StackPanel)FindName("ProjectUsers");

            List<User> users = new ProjectDAO().GetProjectUsers(new SectionDAO().Read(task.SectionID).ProjectId);

            foreach (User user in users)
            {
                UserAvatar userAvatar = new UserAvatar(this, user.Id);

                userAvatar.Uid = user.Id.ToString();
                userAvatar.UserImage.ImageSource = new BitmapImage(new Uri(user.Picture));
                userAvatar.ToolTip = user.Firstname + " " + user.Lastname;

                if (task.AssignedUser != null)
                    if (task.AssignedUser.Id == user.Id)
                        AssignUser(userAvatar, user.Id);

                projectUsers.Children.Add(userAvatar);
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

            ((Button)userAvatar.FindName("AssignMemberToTask")).BorderBrush = (Brush)new BrushConverter().ConvertFrom("#d32f2f");
            assignedUserAvatar = userAvatar;
            assignedUserID = id;
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
                                if (DateTime.TryParse(deadline.Text, out DateTime result))
                                {
                                    return true;
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
            if (ValidateInput())
            {
                string taskName = title.Text;
                string taskDescription = description.Text;
                double taskEstimation = Double.Parse(estimation.Text);
                int taskPriority = int.Parse(priority.Text);
                DateTime taskDeadline = DateTime.Parse(deadline.Text);

                User assignedUser = assignedUserID != 0 ? new UserDAO().Read(assignedUserID) : null;

                task.Name = taskName;
                task.AssignedUser = assignedUser;
                task.Description = taskDescription;
                task.EstimatedTime = taskEstimation;
                task.Priority = taskPriority;
                task.DueDate = taskDeadline;
                
                if (taskDAO.Update(task))
                {
                    taskElement.title.Text = taskName;
                    taskElement.description.Text = taskDescription;

                    if (assignedUser != null)
                    {
                        taskElement.avatar.ImageSource = new BitmapImage(new Uri(assignedUser.Picture));
                        taskElement.UserButton.ToolTip = assignedUser.Firstname + " " + assignedUser.Lastname;
                    } else
                    {
                        taskElement.avatar.ImageSource = null;
                        taskElement.UserButton.ToolTip = "";
                    }
                    
                    this.Close();
                }
            }
        }

        private void Priority_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void ButtonDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskDAO.Delete(task.Id))
            {
                this.Close();
                ((StackPanel) taskElement.Parent).Children.Remove(taskElement);
            }
        }
    }
}
