using project_management.DAO;
using project_management.DTO;
using project_management.Elements;
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
        AddWorkLog addWorkLog;
        Task task;

        public ViewTask(TaskElement taskElement)
        {
            this.taskElement = taskElement;
            task = new TaskDAO().Read(taskElement.taskID);
            addWorkLog = new AddWorkLog();
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
                    addWorkLog.Show();
                    break;
            }

            this.Close();
        }
    }
}
