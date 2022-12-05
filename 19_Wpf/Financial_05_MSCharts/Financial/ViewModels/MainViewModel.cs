using Financial.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;

namespace Financial.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            BarSeriesCollection = new ObservableCollection<Series>();
            LineSeriesCollection = new ObservableCollection<Series>();
            PieSeriesCollection = new ObservableCollection<Series>();
            PolarSeriesCollection = new ObservableCollection<Series>();

            BarChart = new DelegateCommand(OnBarChart);
            LineChart = new DelegateCommand(OnLineChart);
            PieChart = new DelegateCommand(OnPieChart);
            PolarChart = new DelegateCommand(OnPolarChart);
        }

        public ICommand BarChart { get; private set; }
        public ICommand LineChart { get; private set; }
        public ICommand PieChart { get; private set; }
        public ICommand PolarChart { get; private set; }

        public ObservableCollection<Series> BarSeriesCollection { get; set; }
        public ObservableCollection<Series> LineSeriesCollection { get; set; }
        public ObservableCollection<Series> PieSeriesCollection { get; set; }
        public ObservableCollection<Series> PolarSeriesCollection { get; set; }

        public void OnBarChart(object obj)
        {
            double[] data1 = new double[] { 32, 56, 35, 12, 35, 6, 23 };
            double[] data2 = new double[] { 67, 24, 12, 8, 46, 14, 76 };

            BarSeriesCollection.Clear();
            Series ds = new Series();
            ds.ChartType = SeriesChartType.Column;
            ds["DrawingStyle"] = "Cylinder";
            ds.Points.DataBindY(data1);
            BarSeriesCollection.Add(ds);

            ds = new Series();
            ds.ChartType = SeriesChartType.Column;
            ds["DrawingStyle"] = "Cylinder";
            ds.Points.DataBindY(data2);
            BarSeriesCollection.Add(ds);
        }

        public void OnLineChart(object obj)
        {
            LineSeriesCollection.Clear();
            Series ds = new Series();
            ds.ChartType = SeriesChartType.Line;
            ds.BorderDashStyle = ChartDashStyle.Solid;
            ds.MarkerStyle = MarkerStyle.Diamond;
            ds.MarkerSize = 8;
            ds.BorderWidth = 2;
            ds.Name = "Sine";
            for (int i = 0; i < 70; i++)
            {
                double x = i / 5.0;
                double y = 1.1 * Math.Sin(x);
                ds.Points.AddXY(x, y);
            }
            LineSeriesCollection.Add(ds);

            ds = new Series();
            ds.ChartType = SeriesChartType.Line;
            ds.BorderDashStyle = ChartDashStyle.Dash;
            ds.MarkerStyle = MarkerStyle.Circle;
            ds.MarkerSize = 8;
            ds.BorderWidth = 2;
            ds.Name = "Cosine";
            for (int i = 0; i < 70; i++)
            {
                double x = i / 5.0;
                double y = 1.1 * Math.Cos(x);
                ds.Points.AddXY(x, y);
            }
            LineSeriesCollection.Add(ds);
        }

        public void OnPieChart(object obj)
        {
            PieSeriesCollection.Clear();
            Random random = new Random();
            Series ds = new Series();
            for (int i = 0; i < 5; i++)
                ds.Points.AddY(random.Next(10, 50));
            ds.ChartType = SeriesChartType.Pie;
            ds["PointWidth"] = "0.5";
            ds.IsValueShownAsLabel = true;
            ds["BarLabelStyle"] = "Center";
            ds["DrawingStyle"] = "Cylinder";
            PieSeriesCollection.Add(ds);
        }

        public void OnPolarChart(object obj)
        {
            PolarSeriesCollection.Clear();
            Series ds = new Series();
            ds.ChartType = SeriesChartType.Polar;
            ds.BorderWidth = 2;
            for (int i = 0; i < 360; i++)
            {
                double x = 1.0 * i;
                double y = 0.001 + Math.Abs(Math.Sin(2.0 * x * Math.PI / 180.0) * Math.Cos(2.0 * x * Math.PI / 180.0));
                ds.Points.AddXY(x, y);
            }
            PolarSeriesCollection.Add(ds);

        }
    }

}
