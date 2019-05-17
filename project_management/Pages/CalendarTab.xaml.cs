using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
using Separator = LiveCharts.Wpf.Separator;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Dtos;
using project_management.Controllers;
using project_management.DTO;
using project_management.DAO;
using MySql.Data.MySqlClient;
using LiveCharts.Configurations;

namespace project_management.Pages
{
    /// <summary>
    /// Interaction logic for CalendarTab.xaml
    /// </summary>
    public partial class CalendarTab : Page
    {

        private readonly ChartValues<GanttPoint> _values;
        private Project project;
        private List<DTO.Task> taskData;

        public CalendarTab()
        {
            InitializeComponent();

            project = MainController.Instance.Project;

            MySqlDataReader dataReader = new TaskDAO().GetGanttTasks(project.Id);

            if(dataReader != null)
            {
                _values = new ChartValues<GanttPoint>();
                taskData = new List<DTO.Task>();
                while (dataReader.Read())
                {
                    //_values.Add(new GanttPoint(dataReader.IsDBNull(1) ? 0 : ((DateTime)dataReader.GetMySqlDateTime("start_date")).Ticks, dataReader.IsDBNull(2) ? 0 : ((DateTime)dataReader.GetMySqlDateTime("due_date")).Ticks));
                    taskData.Add(new DTO.Task(0, null, null, null, 0, dataReader.GetString(3), dataReader.GetString(4), 0, 0, dataReader.GetBoolean(5), (DateTime)dataReader.GetMySqlDateTime(1), (DateTime)dataReader.GetMySqlDateTime(2), DateTime.MinValue));
                }
            }


            var labels = new List<string>();
            for (int i = 0; i < taskData.Count; i++)
            {
                labels.Add(taskData.ElementAt(i).Name);
                _values.Add(new GanttPoint(taskData.ElementAt(i).StartDate.Ticks, taskData.ElementAt(i).DueDate.Ticks));
            }


            Series = new SeriesCollection
            {
                new RowSeries
                {
                    Values = _values,
                    DataLabels = true,
                    
                }
            };
            Formatter = value => new DateTime((long) value).ToString("dd MMM");

            DataContext = this;

            Gantt.Series = Series;
            Gantt.AxisY.Clear();
            Gantt.AxisY.Add(
                new Axis
                {
                    Labels = labels.ToArray(),
                    Separator = new Separator { Step = 1 },
                    FontSize = 14,
                });

            Gantt.AxisX.Clear();
            Gantt.AxisX.Add(
                new Axis
                {
                    MinValue = project.CreatedAt.AddDays(-1).Ticks,
                    MaxValue = project.DueDate.AddDays(1).Ticks,
                    LabelFormatter = Formatter,
                    FontSize = 14,
                });

            
            Gantt.Height = _values.Count * 50;




        }




        

        public SeriesCollection Series { get; set; }
        public double AxisStep { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }




    }
}