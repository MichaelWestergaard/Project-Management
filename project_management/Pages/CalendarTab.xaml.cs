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

namespace project_management.Pages
{
    /// <summary>
    /// Interaction logic for CalendarTab.xaml
    /// </summary>
    public partial class CalendarTab : Page
    {

        private double _from, _to;
        private readonly ChartValues<GanttPoint> _values;

        public CalendarTab()
        {
            InitializeComponent();

            var now = DateTime.Now;

            _values = new ChartValues<GanttPoint>
            {
                new GanttPoint(now.Ticks, now.AddDays(2).Ticks),
                new GanttPoint(now.AddDays(1).Ticks, now.AddDays(3).Ticks),
                new GanttPoint(now.AddDays(3).Ticks, now.AddDays(5).Ticks),
                new GanttPoint(now.AddDays(5).Ticks, now.AddDays(8).Ticks),
                new GanttPoint(now.AddDays(6).Ticks, now.AddDays(10).Ticks),
                new GanttPoint(now.AddDays(7).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(12).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(10).Ticks, now.AddDays(11).Ticks),
                new GanttPoint(now.AddDays(12).Ticks, now.AddDays(16).Ticks),
                new GanttPoint(now.AddDays(15).Ticks, now.AddDays(17).Ticks),
                new GanttPoint(now.AddDays(18).Ticks, now.AddDays(19).Ticks),
                new GanttPoint(now.Ticks, now.AddDays(2).Ticks),
                new GanttPoint(now.AddDays(1).Ticks, now.AddDays(3).Ticks),
                new GanttPoint(now.AddDays(3).Ticks, now.AddDays(5).Ticks),
                new GanttPoint(now.AddDays(5).Ticks, now.AddDays(8).Ticks),
                new GanttPoint(now.AddDays(6).Ticks, now.AddDays(10).Ticks),
                new GanttPoint(now.AddDays(7).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(12).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(10).Ticks, now.AddDays(11).Ticks),
                new GanttPoint(now.AddDays(12).Ticks, now.AddDays(16).Ticks),
                new GanttPoint(now.AddDays(15).Ticks, now.AddDays(17).Ticks),
                new GanttPoint(now.AddDays(18).Ticks, now.AddDays(19).Ticks),
                new GanttPoint(now.Ticks, now.AddDays(2).Ticks),
                new GanttPoint(now.AddDays(1).Ticks, now.AddDays(3).Ticks),
                new GanttPoint(now.AddDays(3).Ticks, now.AddDays(5).Ticks),
                new GanttPoint(now.AddDays(5).Ticks, now.AddDays(8).Ticks),
                new GanttPoint(now.AddDays(6).Ticks, now.AddDays(10).Ticks),
                new GanttPoint(now.AddDays(7).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(12).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(10).Ticks, now.AddDays(11).Ticks),
                new GanttPoint(now.AddDays(12).Ticks, now.AddDays(16).Ticks),
                new GanttPoint(now.AddDays(15).Ticks, now.AddDays(17).Ticks),
                new GanttPoint(now.AddDays(18).Ticks, now.AddDays(19).Ticks),
                new GanttPoint(now.Ticks, now.AddDays(2).Ticks),
                new GanttPoint(now.AddDays(1).Ticks, now.AddDays(3).Ticks),
                new GanttPoint(now.AddDays(3).Ticks, now.AddDays(5).Ticks),
                new GanttPoint(now.AddDays(5).Ticks, now.AddDays(8).Ticks),
                new GanttPoint(now.AddDays(6).Ticks, now.AddDays(10).Ticks),
                new GanttPoint(now.AddDays(7).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(12).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(10).Ticks, now.AddDays(11).Ticks),
                new GanttPoint(now.AddDays(12).Ticks, now.AddDays(16).Ticks),
                new GanttPoint(now.AddDays(15).Ticks, now.AddDays(17).Ticks),
                new GanttPoint(now.AddDays(18).Ticks, now.AddDays(19).Ticks)
            };


            Series = new SeriesCollection
            {
                new RowSeries
                {
                    Values = _values
                }
            };
            Formatter = value => new DateTime((long) value).ToString("dd MMM");

            var labels = new List<string>();
            for (int i = 0; i < _values.Count; i++)
            {
                labels.Add("Task " + i);
            }

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
                    MinValue = now.AddDays(-15).Ticks,
                    MaxValue = now.AddDays(30).Ticks,
                    LabelFormatter = Formatter,
                    FontSize = 14,
                });

            
            Gantt.Height = _values.Count * 50;




        }




        

        public SeriesCollection Series { get; set; }
        public double AxisStep { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }/*

        public double From
        {
            get { return _from; }
            set { _from = value; }
        }

        public double To
        {
            get { return _to; }
            set { _to = value; }
        }

    */



    }
}