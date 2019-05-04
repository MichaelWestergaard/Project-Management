using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using project_management.Controllers;
using project_management.DAO;
using project_management.DTO;
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

namespace project_management.Elements
{
    /// <summary>
    /// Interaction logic for LineChart.xaml
    /// </summary>
    public partial class BurndownChart : UserControl
    {
        public BurndownChart()
        {
            InitializeComponent();

            Project project = MainController.Instance.Project;
            int days = 0, daysFromStart = 0;
            double totalWork = 0;

            if(project.DueDate != DateTime.MinValue)
            {

                MySqlDataReader dataReader = new ProjectDAO().GetBurndwonChartData(project.Id);

                if (dataReader.Read())
                {
                    days = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("ProjectLength");
                    totalWork = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("TotalWork");
                    daysFromStart = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt16("DaysFromStart");
                }

                // Get values for target line
                ChartValues<double> target = new ChartValues<double>();

                for(int i = 0; i <= days; i++)
                {
                    if (target.Count == 0)
                    {
                        target.Add(totalWork);
                    }
                    else if (i == days)
                    {
                        target.Add(0);
                    }
                    else
                    {
                        target.Add(Math.Round(target[i - 1] - (totalWork / days),2));
                    }
                }

                // Get values for actual work done
                ChartValues<double> actual = new ChartValues<double>();
                MySqlDataReader dataReaderActual = new WorkLogDAO().GetWorkLogByProject(project.Id);

                List<WorkLog> workLogs = new List<WorkLog>();

                if (dataReaderActual != null)
                {
                    while (dataReaderActual.Read())
                    {
                        workLogs.Add(new WorkLog(null, 0, dataReaderActual.IsDBNull(0) ? 0 : dataReaderActual.GetInt16("Work"), (DateTime)dataReaderActual.GetMySqlDateTime("Date")));
                    }

                    for (int i = 0; i <= daysFromStart; i++)
                    {
                        if (actual.Count == 0)
                        {
                            actual.Add(totalWork);
                        }
                        else
                        {
                            bool foundWork = false;

                            foreach (WorkLog work in workLogs)
                            {
                                if (work.CreatedAt.Date.Equals(project.CreatedAt.AddDays(i).Date))
                                {
                                    actual.Add(actual[actual.Count - 1]-work.Work);
                                    workLogs.Remove(work);
                                    foundWork = true;
                                    break;
                                }

                                if (work.Equals(workLogs.Last()))
                                {
                                    foundWork = false;
                                }
                            }

                            if (!foundWork)
                                actual.Add(actual[actual.Count - 1]);
                        }
                    }
                }

                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Gudieline",
                        Values = target,
                        LineSmoothness = 0
                    },
                    new LineSeries
                    {
                        Title = "Arbejde tilbage",
                        Values = actual,
                        LineSmoothness = 0
                    }
                };
            
                DataContext = this;

                Chart.Series = SeriesCollection;
                Chart.AxisY.Clear();
                Chart.AxisY.Add(
                    new Axis
                    {
                        MinValue = 0,
                        Title = "Project Estimat (dage)",
                        Separator = new Separator { Stroke = new Utilities().GetColor("#546e7a") },
                    });

                Chart.AxisX.Clear();
                Chart.AxisX.Add(
                    new Axis
                    {
                        MinValue = 0,
                        MaxValue = 7,
                        Title = "Tid (dage)",
                        Separator = new Separator { Step = 1, Stroke = new Utilities().GetColor("#546e7a") },
                    });


            }

        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}
