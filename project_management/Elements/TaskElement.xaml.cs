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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            //TODO: add different windows here
            switch (menuItem.Uid)
            {
                case "1":
                    EditTask editTask = new EditTask(this);
                    editTask.Show();
                    break;

                case "2":
                    AddWorkLog addWorkLog = new AddWorkLog(this);
                    addWorkLog.Show();
                    break;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid taskElementGrid = sender as Grid;
            DataObject dataObject = new DataObject();
            dataObject.SetData("Task", this);
            dataObject.SetData("SectionFrom", this.Parent);
            DragDrop.DoDragDrop(taskElementGrid, dataObject, DragDropEffects.Move);
        }

        public void UpdateProgress(double percentage)
        {
            Progress.Value = percentage;
            Progress.ToolTip = percentage + "% færdig";
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
        }
    }
}
