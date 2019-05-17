using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using project_management.Controllers;
using project_management.DAO;
using project_management.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications.Messages;

namespace project_management.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {

        MySQLConnector mySQLConnector = MySQLConnector.Instance;
        Utilities utilities = new Utilities();
        MainController mainController = MainController.Instance;
        DashboardQuickStats tasksLeftElement, completedTaskElement, yourTasksElement, deadlineElement;
        StackPanel quickStatsList;
        Grid grid;

        private BackgroundWorker worker;

        int tasksLeft, tasksCompleted, yourTasks;
        string daysLeft;

        public Dashboard()
        {
            InitializeComponent();
            mainController.Dashboard = this;

            try
            {
                if(mainController.Project != null)
                {
                    using (MySqlDataReader dataReader = new ProjectDAO().GetDashboardStats(mainController.Project.Id))
                    {
                        if (dataReader.Read())
                        {
                            tasksLeft = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("TasksLeft");
                            tasksCompleted = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt16("TasksCompleted");
                            yourTasks = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt16("YourTasks");
                            daysLeft = dataReader.IsDBNull(5) ? "Ingen" : dataReader.GetInt16("DaysLeft") + " dage";
                        }
                    }
                    
                    mySQLConnector.CloseConnections();

                    SetupQuickStats();
                    SetupCharts();
                    
                    worker = new BackgroundWorker();
                    worker.DoWork += WorkerUpdater;
                    Timer timer = new Timer(60000);
                    timer.Elapsed += TimerElapsed;
                    timer.Start();
                }
            }
            catch
            {
                throw;
            }
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        void WorkerUpdater(object sender, DoWorkEventArgs e)
        {
            try
            {
                UpdatePage();
            }
            catch
            {
                throw;
            }
        }

        public void UpdatePage()
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (mainController.Project != null)
                    {
                        using (MySqlDataReader dataReader = new ProjectDAO().GetDashboardStats(mainController.Project.Id))
                        {
                            if (dataReader.Read())
                            {
                                tasksLeft = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("TasksLeft");
                                tasksCompleted = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt16("TasksCompleted");
                                yourTasks = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt16("YourTasks");
                                daysLeft = dataReader.IsDBNull(5) ? "Ingen" : dataReader.GetInt16("DaysLeft") + " dage";
                            }
                        }

                        mySQLConnector.CloseConnections();

                        UpdateQuickStats();
                        UpdateCharts();
                    }
                }
                catch (Exception exception)
                {
                    utilities.GetNotifier().ShowError(utilities.HandleException(exception));
                }
            });
        }

        private void SetupCharts()
        {
            grid = new Grid();
            grid.Width = 900;
            grid.Children.Add(new BurndownChart());
            grid.Children.Add(new PieChart(tasksLeft, tasksCompleted));
            ChartList.Children.Add(grid);
        }

        private void SetupQuickStats()
        {

            quickStatsList = (StackPanel)FindName("QuickStatsList");

            tasksLeftElement = (DashboardQuickStats)quickStatsList.Children[0];
            completedTaskElement = (DashboardQuickStats)quickStatsList.Children[1];
            yourTasksElement = (DashboardQuickStats)quickStatsList.Children[2];
            deadlineElement = (DashboardQuickStats)quickStatsList.Children[3];

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

        private void UpdateQuickStats()
        {
            tasksLeftElement.Value.Text = tasksLeft.ToString();
            completedTaskElement.Value.Text = tasksCompleted.ToString();
            yourTasksElement.Value.Text = yourTasks.ToString();
            deadlineElement.Value.Text = daysLeft;
        }

        private void UpdateCharts()
        {
            ((BurndownChart)grid.Children[0]).UpdateChart();
            ((PieChart)grid.Children[1]).UpdateChart(tasksLeft, tasksCompleted);
        }

    }
}
