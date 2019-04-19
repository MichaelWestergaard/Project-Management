using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;

namespace project_management
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Page
    {
        private ViewModels.OverviewModel viewModel;
        public Overview()
        {
            viewModel = new ViewModels.OverviewModel();
            DataContext = viewModel;
            
            InitializeComponent();
            List<Userss> items = new List<Userss>();
            items.Add(new Userss() { Name = "John Doe", Age = 42 });
            items.Add(new Userss() { Name = "Jane Doe", Age = 39 });
            yourTasks.ItemsSource = items;

            // Draw a simple graph.
                const double margin = 10;
                double xmin = margin;
                double xmax = canGraph.Width - margin;
                double ymin = margin;
                double ymax = canGraph.Height - margin;
                const double step = 10;

                // Make the X axis.
                GeometryGroup xaxis_geom = new GeometryGroup();
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(0, ymax), new Point(canGraph.Width, ymax)));
                for (double x = xmin + step;
                    x <= canGraph.Width - step; x += step)
                {
                    xaxis_geom.Children.Add(new LineGeometry(
                        new Point(x, ymax - margin / 2),
                        new Point(x, ymax + margin / 2)));
                }

                Path xaxis_path = new Path();
                xaxis_path.StrokeThickness = 1;
                xaxis_path.Stroke = Brushes.Black;
                xaxis_path.Data = xaxis_geom;

                canGraph.Children.Add(xaxis_path);

                // Make the Y ayis.
                GeometryGroup yaxis_geom = new GeometryGroup();
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin, 0), new Point(xmin, canGraph.Height)));
                for (double y = step; y <= canGraph.Height - step; y += step)
                {
                    yaxis_geom.Children.Add(new LineGeometry(
                        new Point(xmin - margin / 2, y),
                        new Point(xmin + margin / 2, y)));
                }

                Path yaxis_path = new Path();
                yaxis_path.StrokeThickness = 1;
                yaxis_path.Stroke = Brushes.Black;
                yaxis_path.Data = yaxis_geom;

                canGraph.Children.Add(yaxis_path);

                // Make some data sets.
                Brush[] brushes = { Brushes.Red, Brushes.Blue, Brushes.Yellow };
                Random rand = new Random();
                for (int data_set = 0; data_set < 3; data_set++)
                {
                    int last_y = rand.Next((int)ymin, (int)ymax);

                    PointCollection points = new PointCollection();
                    for (double x = xmin; x <= xmax; x += step)
                    {
                        last_y = rand.Next(last_y - 10, last_y + 10);
                        if (last_y < ymin) last_y = (int)ymin;
                        if (last_y > ymax) last_y = (int)ymax;
                        points.Add(new Point(x, last_y));
                    }

                    Polyline polyline = new Polyline();
                    polyline.StrokeThickness = 1;
                    polyline.Stroke = brushes[data_set];
                    polyline.Points = points;

                    canGraph.Children.Add(polyline);
                
                }
         









        }


    


    }









    public class Userss
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Mail { get; set; }
    }





}
