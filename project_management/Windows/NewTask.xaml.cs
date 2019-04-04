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

            Console.WriteLine("Section id " + sectionID);

            InitializeComponent();

            Task task = new Task(null, null, new UserDAO().Read(1), sectionID, "test", "gg", 12.0, 10, new DateTime());

            Console.WriteLine(task);

            if (taskDAO.Create(task))
                currentSection.Children.Add(new TaskElement());
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
            Task task = new Task(null, null, new UserDAO().Read(1), sectionID, title.Text, description.Text, Double.Parse(estimation.Text), int.Parse(priority.Text), DateTime.Parse(deadline.Text));

            Console.WriteLine(task);

            if (taskDAO.Create(task))
                currentSection.Children.Add(new TaskElement());
        }
    }
}
