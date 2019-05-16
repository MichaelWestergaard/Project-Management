using project_management.DAO;
using project_management.DTO;
using project_management.Elements;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications.Messages;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for ViewTask.xaml
    /// </summary>
    public partial class ViewTask : Window
    {
        TaskElement taskElement;
        Task task;
        WorkLogDAO workLogDAO = new WorkLogDAO();
        UserDAO userDAO = new UserDAO();
        List<WorkLog> worklogs = new List<WorkLog>();
        List<WorkLogItems> workLogsItems = new List<WorkLogItems>();
        Utilities utilities = new Utilities();

        public ViewTask(TaskElement taskElement)
        {
            InitializeComponent();
            try
            {
                this.taskElement = taskElement;
                task = new TaskDAO().Read(taskElement.taskID);


                worklogs = workLogDAO.GetList(task.Id);

                foreach (WorkLog workLog in worklogs)
                {
                    workLogsItems.Add(new WorkLogItems() { Work = workLog.Work, Name = workLog.AssignedUser.Firstname + " " + workLog.AssignedUser.Lastname, Date = workLog.CreatedAt.Day.ToString() + "-" + workLog.CreatedAt.Month.ToString() + "-" + workLog.CreatedAt.Year.ToString() });
                }

                workloads.ItemsSource = workLogsItems;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button source = (Button)e.Source;
            string action = source.Name;
            
            switch (action)
            {
                case "EditTask":
                    if (utilities.CheckOpen(typeof(EditTask)) == false)
                    {
                        new EditTask(taskElement).Show();
                    }
                    break;

                case "WorkLoad":
                    if (utilities.CheckOpen(typeof(AddWorkLog)) == false)
                    {
                        new AddWorkLog(taskElement).Show();
                    }
                    break;
            }

            this.Close();
        }

        public class WorkLogItems
        {
            public double Work { get; set; }

            public string Name { get; set; }

            public string Date { get; set; }
        }
        
    }
}
