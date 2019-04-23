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
        private int taskID = 0;


        public AddWorkLog(TaskElement taskElement)
        {
            this.taskElement = taskElement;



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

        private bool ValidateInput()
        {
            return true;
        }
    }
}
