using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using project_management.Controllers;
using project_management.DAO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project_management.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        Utilities utilities = new Utilities();
        MainController mainController = MainController.Instance;
        DashboardQuickStats tasksLeftElement, completedTaskElement, yourTasksElement, deadlineElement;

        int tasksLeft, tasksCompleted, yourTasks;
        string daysLeft;

        public Dashboard()
        {
            InitializeComponent();

            if(mainController.Project != null)
            {
                MySqlDataReader dataReader = new ProjectDAO().GetDashboardStats(mainController.Project.Id);

                if (dataReader.Read())
                {
                    tasksLeft = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("TasksLeft");
                    tasksCompleted = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt16("TasksCompleted");
                    yourTasks = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt16("YourTasks");
                    daysLeft = dataReader.IsDBNull(5) ? "Ingen" : dataReader.GetInt16("DaysLeft") + " dage";
                }

                SetupQuickStats();
                SetupCharts();
            }

        }

        private void SetupCharts()
        {
            Grid grid = new Grid();
            grid.Width = 900;
            grid.Children.Add(new BurndownChart());
            grid.Children.Add(new PieChart(tasksLeft, tasksCompleted));
            ChartList.Children.Add(grid);
        }

        private void SetupQuickStats()
        {

            StackPanel quickStatsList = (StackPanel) FindName("QuickStatsList");

            tasksLeftElement        = (DashboardQuickStats) quickStatsList.Children[0];
            completedTaskElement    = (DashboardQuickStats) quickStatsList.Children[1];
            yourTasksElement        = (DashboardQuickStats) quickStatsList.Children[2];
            deadlineElement         = (DashboardQuickStats) quickStatsList.Children[3];

            tasksLeftElement.Value.Text = tasksLeft.ToString();
            tasksLeftElement.Icon.Kind = PackIconKind.FileDocumentBoxMultiple;
            tasksLeftElement.StatCard.Background = utilities.GetColor("#0091ea");
            tasksLeftElement.Title.Text = "opgaver tilbage";
            
            completedTaskElement.Value.Text = tasksCompleted.ToString();
            completedTaskElement.Icon.Kind = PackIconKind.DoneAll;
            completedTaskElement.StatCard.Background = utilities.GetColor("#00b8d4");
            completedTaskElement.Title.Text = "afsluttede opgaver";
            
            yourTasksElement.Value.Text = yourTasks.ToString();
            yourTasksElement.Icon.Kind = PackIconKind.User;
            yourTasksElement.StatCard.Background = utilities.GetColor("#00bfa5");
            yourTasksElement.Title.Text = "dine opgaver";
            
            deadlineElement.Value.Text = daysLeft;
            deadlineElement.Icon.Kind = PackIconKind.CalendarRange;
            deadlineElement.StatCard.Background = utilities.GetColor("#546e7a");
            deadlineElement.Title.Text = "deadline";
        }
        
    }
}
