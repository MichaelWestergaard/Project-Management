using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
//using project_management.Annotations;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace project_management.ViewModels
{

    public class OverviewModel : INotifyPropertyChanged
    {
        private PlotModel piechart, burndown;
        public PlotModel Piechart
        {
            get { return piechart; }
            set { piechart = value; OnPropertyChanged("Piechart"); }
        }

        public PlotModel Burndown
        {
            get { return burndown; }
            set { burndown = value; OnPropertyChanged("Burndown"); }
        }

        public OverviewModel()
        {
            Piechart = new PlotModel();
            Burndown = new PlotModel();
            SetUpModels();
        }

        private void SetUpModels()
        {
            SetUpPiechart();
            SetUpBurndown();
        }

        private void SetUpPiechart()
        {
            dynamic seriesP1 = new PieSeries { };

            seriesP1.Slices.Add(new PieSlice("Completed", 25) { IsExploded = true });
            seriesP1.Slices.Add(new PieSlice("In Progress", 75) { IsExploded = true });

            Piechart.Series.Add(seriesP1);
        }

        private void SetUpBurndown()
        {
            //Setup the axis
            var startDate = DateTime.Now.AddDays(-10).Date;
            var endDate = DateTime.Now.Date;

            double minValue = DateTimeAxis.ToDouble(startDate);
            double maxValue = DateTimeAxis.ToDouble(endDate);

            var xAxis = new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "dd/MM/yy", Title = "Datoer", Minimum = minValue, Maximum = maxValue, IntervalLength = 35, IsPanEnabled = false, IsZoomEnabled = false };
            Burndown.Axes.Add(xAxis);

            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Opgaver", Minimum = 0, Maximum = 4, IsPanEnabled = false, IsZoomEnabled = false };
            Burndown.Axes.Add(yAxis);


            //Setup the actual data
            var burndownSeries = new LineSeries { StrokeThickness = 2, CanTrackerInterpolatePoints = false };

            burndownSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(-10).Date), 4));
            burndownSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(-8).Date), 3));
            burndownSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(-4).Date), 2));
            burndownSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(-2).Date), 1));
            burndownSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.Date), 0));

            Burndown.Series.Add(burndownSeries);


            //Setup the required pace
            var burndownLinear = new LineSeries { StrokeThickness = 1, CanTrackerInterpolatePoints = false };

            int timespan = (endDate - startDate).Days;
            double timePerDay = 4.0 / timespan;

            Console.WriteLine("Timespan: " + timespan);
            Console.WriteLine("Time Per Day: " + timePerDay);

            for(int i = 0; i <= timespan; i++)
            {
                burndownLinear.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays((0-i)).Date), i*timePerDay));
            }

            Burndown.Series.Add(burndownLinear);


        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
