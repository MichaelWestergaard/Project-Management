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
        private Utilities utilities = new Utilities();

        public TaskElement(int taskID)
        {
            this.taskID = taskID;
            InitializeComponent();
        }

        private void TaskID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
            if (utilities.CheckOpen(typeof(ViewTask)) == false)
            {
                ViewTask viewTask = new ViewTask(this);
                viewTask.Show();
            } 
           
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            //TODO: add different windows here
            switch (menuItem.Uid)
            {
                case "1":
                    if (utilities.CheckOpen(typeof(EditTask)) == false)
                    {
                        new EditTask(this).Show();
                    }
                    break;

                case "2":
                    if (utilities.CheckOpen(typeof(AddWorkLog)) == false)
                    {
                        new AddWorkLog(this).Show();
                    }
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
