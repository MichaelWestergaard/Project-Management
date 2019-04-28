using project_management.DAO;
using project_management.DTO;
using project_management.Elements;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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


        public ViewTask(TaskElement taskElement)
        {
            this.taskElement = taskElement;
            task = new TaskDAO().Read(taskElement.taskID);
            InitializeComponent();

            worklogs = workLogDAO.GetList(task.Id);
            System.Console.WriteLine("Hvor mange elementer : " + worklogs.Capacity);
            foreach (WorkLog workLog in worklogs)
            {
                workLogsItems.Add(new WorkLogItems() { Work = workLog.Work, Name = workLog.AssignedUser.Firstname + " " + workLog.AssignedUser.Lastname, Date = workLog.CreatedAt.Day.ToString() + "-" + workLog.CreatedAt.Month.ToString() + "-" + workLog.CreatedAt.Year.ToString() });
            }


            workloads.ItemsSource = workLogsItems;
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
                    EditTask editTask = new EditTask(taskElement);
                    editTask.Show();
                    break;

                case "WorkLoad":
                    AddWorkLog addWorkLog = new AddWorkLog(taskElement);
                    addWorkLog.Show();
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
