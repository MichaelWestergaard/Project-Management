using project_management.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project_management.Elements
{
    /// <summary>
    /// Interaction logic for TaskElement.xaml
    /// </summary>
    public partial class TaskElement : UserControl
    {
        public int taskID;

        public TaskElement(int taskID)
        {
            this.taskID = taskID;
            InitializeComponent();
        }

        private void TaskID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewTask viewTask = new ViewTask(this);
            viewTask.Show();
            viewTask.Activate();
        }
    }
}
