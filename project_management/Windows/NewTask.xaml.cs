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

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for NewTask.xaml
    /// </summary>
    public partial class NewTask : Window
    {
        private StackPanel currentSection;


        public NewTask()
        {
            InitializeComponent();
        }

        public NewTask(StackPanel currentSection)
        {
            this.currentSection = currentSection;

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

        private void ButtnCreateTask_Click(object sender, RoutedEventArgs e)
        {
            
            Task task = new Task(null, null, new UserDAO().read(1), title.Text, description.Text, Double.Parse(estimation.Text), priority.Text, deadline.Text);
            currentSection.Children.Add(new TaskElement());
        }
    }
}
