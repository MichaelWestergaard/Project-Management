using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using project_management.DTO;
using project_management.Elements;
using project_management.DAO;
using ToastNotifications.Messages;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for AddWorkLog.xaml
    /// </summary>
    public partial class AddWorkLog : Window
    {
        private TaskDAO taskDAO = new TaskDAO();
        private WorkLogDAO workLogDAO = new WorkLogDAO();
        private TaskElement taskElement;
        private int assignedUserID = 1;
        private Board board = new Board();

        StackPanel taskList;
        StackPanel completedTaskList;


        public AddWorkLog(TaskElement taskElement)
        {
            this.taskElement = taskElement;
            InitializeComponent();

            taskList = (StackPanel)taskElement.Parent;
            StackPanel lastStackPanel = (StackPanel)((StackPanel)taskElement.Parent).Children[taskList.Children.Count - 1];
            completedTaskList = ((StackPanel)lastStackPanel.Children[lastStackPanel.Children.Count - 1]);
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtnUpdateWork_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {

                double workLoad = Double.Parse(estimation.Text);
                DateTime workDato = DateTime.Parse(dato.Text);

                User assignedUser = assignedUserID != 0 ? new UserDAO().Read(assignedUserID) : null;
                int taskID = taskElement.taskID;

                WorkLog workLog = new WorkLog(assignedUser, taskID, workLoad, workDato);

                int workLogID = workLogDAO.CreateWork(workLog);

                this.Close();
            }
        }

        private void ButtnComplete_Click(object sender, RoutedEventArgs e)
        {
            Task task = taskDAO.Read(taskElement.taskID);

            if (task.Completed == false) {
                task.Completed = true;
                taskDAO.Update(task);
                taskElement.Opacity = 0.5;
                taskList.Children.Remove(taskElement);
                completedTaskList.Children.Add(taskElement);


            }
            else
            {
                task.Completed = false;
                taskDAO.Update(task);
                taskElement.Opacity = 1;
                completedTaskList.Children.Remove(taskElement);
                //  taskList.Children.Insert(taskList.Children.Count - 1, taskElement);
                taskList.Children.Add(taskElement);


            }



            this.Close();
        }




        private bool ValidateInput()
        {
            if (estimation.Text != "" && double.TryParse(estimation.Text, out double resultEstimation))
            {
                if (dato.Text != "")
                {
                    if (DateTime.TryParse(dato.Text, out DateTime result))
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
                new Utilities().GetNotifier().ShowError("Udfyld estimation");
            }
            return false;
        }
    }
}
           
