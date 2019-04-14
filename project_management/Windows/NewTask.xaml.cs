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


        public NewTask()
        {
            InitializeComponent();
        }

        public NewTask(StackPanel currentSection)
        {
            this.currentSection = currentSection;

            string sectionName = currentSection.Name;

            sectionID = int.Parse(sectionName.Remove(0, "Section".Length));
            
            InitializeComponent();
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AssignMemberToTask_Click(object sender, RoutedEventArgs e)
        {

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

                User assignedUser = new UserDAO().Read(1);


                Task task = new Task(null, null, assignedUser, sectionID, taskName, taskDescription, taskEstimation, taskPriority, taskDeadline);

                int taskID = taskDAO.CreateTask(task);
                 

                if (taskDAO.Read(taskID) != null)
                {
                    TaskElement taskElement = new TaskElement();

                    taskElement.TaskID.Name = "Task" + taskID;

                    taskElement.title.Text = taskName;
                    taskElement.description.Text = taskDescription;
                    taskElement.avatar.ImageSource = new BitmapImage(new Uri(assignedUser.Picture));
                    taskElement.UserButton.ToolTip = assignedUser.Firstname + " " + assignedUser.Lastname;

                    currentSection.Children.Add(taskElement);
                    this.Close();
                }
            }
        }

        private void Priority_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
        
    }
}
