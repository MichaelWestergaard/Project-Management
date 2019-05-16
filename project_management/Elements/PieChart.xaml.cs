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
using LiveCharts;
using LiveCharts.Wpf;

namespace project_management.Elements
{
    /// <summary>
    /// Interaction logic for PieChart.xaml
    /// </summary>
    public partial class PieChart : UserControl
    {
        int tasksLeft, tasksCompleted;

        public PieChart(int tasksLeft, int tasksCompleted)
        {
            this.tasksLeft = tasksLeft;
            this.tasksCompleted = tasksCompleted;

            InitializeComponent();


            HorizontalAlignment = HorizontalAlignment.Right;

            PointLabel = chartPoint => string.Format("{1}%", chartPoint.Y, Math.Round(chartPoint.Participation * 100));

            DataContext = this;
            
            PieChartElement.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Opgaver tilbage",
                    Values = new ChartValues<int> { tasksLeft },
                    DataLabels = true,
                    LabelPoint = PointLabel,
                    FontSize = 14,
                    ToolTip = "Opgaver tilbage"
                },
                new PieSeries
                {
                    Title = "Udførte opgaver",
                    Values = new ChartValues<int> { tasksCompleted },
                    DataLabels = true,
                    LabelPoint = PointLabel,
                    FontSize = 14,
                    ToolTip = "Udførte opgaver"
                }
            };
            

        }

        public void UpdateChart(int tasksLeft, int tasksCompleted)
        {
            if(this.tasksLeft != tasksLeft || this.tasksCompleted != tasksCompleted)
            {
                this.tasksLeft = tasksLeft;
                this.tasksCompleted = tasksCompleted;
                
                PieChartElement.Series[0].Values = new ChartValues<int> { tasksLeft };
                PieChartElement.Series[1].Values = new ChartValues<int> { tasksCompleted };
            }
        }

        public Func<ChartPoint, string> PointLabel { get; set; }
        
    }
}
