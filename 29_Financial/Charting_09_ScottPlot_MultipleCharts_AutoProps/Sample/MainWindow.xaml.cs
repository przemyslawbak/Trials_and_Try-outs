using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sample
{
    //https://scottplot.net/cookbook/4.1/category/multi-axis/#additional-y-axis
    //https://scottplot.net/cookbook/4.1/category/plottable-axis-line-and-span/#position-labels-on-additional-axes
    public partial class MainWindow : Window
    {
        int _labelSpacingDivider = 6;
        Crosshair Crosshair;
        string[] _dataSetX = Array.Empty<string>();
        double[] _dataSetYMain = Array.Empty<double>();
        double[] _dataSetY2 = Array.Empty<double>();
        double[] _dataSetY3 = Array.Empty<double>();
        double[] _positions = Array.Empty<double>();

        public MainWindow()
        {
            InitializeComponent();
            _dataSetYMain = GetSomeData().Select(x => x.MainValue).ToArray();
            _dataSetY2 = GetSomeData().Select(x => x.Value2).ToArray();
            _dataSetY3 = GetSomeData().Select(x => x.Value3).ToArray();
            _dataSetX = GetSomeData().Select(x => x.Time.ToString("dd-MM HH:mm")).ToArray();
            _positions = DataGen.Consecutive(_dataSetX.Length);

            var xxx = GetDataFromApi(new List<string>() { "Index_SPXINDEX", "DataTypeA", "DataTypeB", "DataTypeC", "DataTypeD" });

            var valMain = wpfPlot1.Plot.AddSignalXY(_positions, _dataSetYMain);
            wpfPlot1.Plot.XAxis.DateTimeFormat(true);
            Crosshair = wpfPlot1.Plot.AddCrosshair(_positions[0], _dataSetYMain[0]);
            Crosshair.VerticalLine.PositionFormatter = pos => pos > _positions.Last() ? _dataSetX.Last() : (pos < _positions.First() ? _dataSetX.First() : _dataSetX[(int)pos]);
            wpfPlot1.Plot.XAxis.TickLabelStyle(rotation: 45);

            valMain.YAxisIndex = wpfPlot1.Plot.LeftAxis.AxisIndex;
            wpfPlot1.Plot.YAxis.Label("Main Value");
            wpfPlot1.Plot.YAxis.Color(valMain.Color);

            var val2 = wpfPlot1.Plot.AddSignalXY(_positions, _dataSetY2);
            var yAxis2 = wpfPlot1.Plot.AddAxis(ScottPlot.Renderable.Edge.Left);
            val2.YAxisIndex = yAxis2.AxisIndex;
            yAxis2.Label("Value 2");
            yAxis2.Color(val2.Color);

            var val3 = wpfPlot1.Plot.AddSignalXY(_positions, _dataSetY3);
            var yAxis3 = wpfPlot1.Plot.AddAxis(ScottPlot.Renderable.Edge.Left);
            val3.YAxisIndex = yAxis3.AxisIndex;
            yAxis3.Label("Value 3");
            yAxis3.Color(val3.Color);

            wpfPlot1.AxesChanged += OnAxesChanged;
            wpfPlot1.Refresh();
        }

        private void OnAxesChanged(object sender, RoutedEventArgs e)
        {
            var ticksDisplayed = wpfPlot1.Plot.XAxis.GetTicks().Length;
            var labelsSpacing = ticksDisplayed / _labelSpacingDivider;
            if (labelsSpacing != 0)
            {
                string[] labels = _dataSetX.Select((x, i) => i % labelsSpacing == 0 ? x : "").ToArray();
                wpfPlot1.Plot.XAxis.ManualTickPositions(_positions, labels);
            }
            else
            {
                labelsSpacing = _dataSetX.Length / _labelSpacingDivider;
                string[] labels = _dataSetX.Select((x, i) => i % labelsSpacing == 0 ? x : "").ToArray();
                wpfPlot1.Plot.XAxis.ManualTickPositions(_positions, labels);
            }
        }

        private List<DataResultModel> GetDataFromApi(List<string> dataTypes)
        {
            var xxx = new List<DataResultModel>();
            var bogus = new Bogus.Faker<DataResultModel>();
            var baseResultValueDictionary = new Dictionary<string, decimal>()
            {
                { "Index_SPXINDEX", 3485.00M },
                { "DataTypeA", 0.294M },
                { "DataTypeB", 10.9M },
                { "DataTypeC", 0.017M },
                { "DataTypeD", 183.15M },
            };
            var baseMinValRangeDictionary = new Dictionary<string, decimal>()
            {
                { "Index_SPXINDEX", -30M },
                { "DataTypeA", -0.15M },
                { "DataTypeB", -1M },
                { "DataTypeC", -0.02M },
                { "DataTypeD", -15M },
            };
            var baseMaxValRangeDictionary = new Dictionary<string, decimal>()
            {
                { "Index_SPXINDEX", 30M },
                { "DataTypeA", 0.15M },
                { "DataTypeB", 1M },
                { "DataTypeC", 0.02M },
                { "DataTypeD", 15M },
            };
            var releaseDictionary = new Dictionary<string, bool>()
            {
                { "Index_SPXINDEX", true },
                { "DataTypeA", false },
                { "DataTypeB", true },
                { "DataTypeC", false },
                { "DataTypeD", true },
            };

            for (int i = 0; i < dataTypes.Count(); i++)
            {
                for (int j = 0; j < 100; j++)
                {

                    bogus.CustomInstantiator(faker => new DataResultModel()
                    {
                        DataGuid = Guid.NewGuid(),
                        DataType = dataTypes[i],
                        IndexName = "SPX",
                        Release = releaseDictionary[dataTypes[i]],
                        ResultValue = baseResultValueDictionary[dataTypes[i]] + faker.Random.Decimal(baseMinValRangeDictionary[dataTypes[i]], baseMaxValRangeDictionary[dataTypes[i]]),
                        UtcTimeStamp = DateTime.UtcNow,
                    });

                    xxx.Add(bogus.Generate());
                }
            }

            return xxx;
        }

        private IEnumerable<string> GetSelectedDataTypes()
        {
            var xxx = new List<DataTypeViewModel>()
            {
                new DataTypeViewModel() { DataType = "Index_SPXINDEX", DataValue = "3485", Interval = 15, IsSelected = true, Multiplier = -1 },
                new DataTypeViewModel() { DataType = "DataTypeA", DataValue = "0.294", Interval = 15, IsSelected = false, Multiplier = 1 },
                new DataTypeViewModel() { DataType = "DataTypeB", DataValue = "10.9", Interval = 15, IsSelected = true, Multiplier = -1 },
                new DataTypeViewModel() { DataType = "DataTypeC", DataValue = "0.017", Interval = 15, IsSelected = true, Multiplier = 1 },
                new DataTypeViewModel() { DataType = "DataTypeD", DataValue = "183.15", Interval = 15, IsSelected = false, Multiplier = -1 },
            };

            return xxx.Where(x => x.IsSelected).Select(x => x.DataType);
        }

        private List<SomeDataModel> GetSomeData()
        {
            var xxx = new List<SomeDataModel>()
            {
                new SomeDataModel() { MainValue = 3592, Time = DateTime.Now.AddMinutes(-19).AddDays(-1), Value2 = 18.6, Value3 = 0.010 },
                new SomeDataModel() { MainValue = 3591, Time = DateTime.Now.AddMinutes(-18).AddDays(-1), Value2 = 18.3, Value3 = 0.011 },
                new SomeDataModel() { MainValue = 3582, Time = DateTime.Now.AddMinutes(-17).AddDays(-1), Value2 = 17.4, Value3 = 0.012 },
                new SomeDataModel() { MainValue = 3543, Time = DateTime.Now.AddMinutes(-16).AddDays(-1), Value2 = 17.5, Value3 = 0.013 },
                new SomeDataModel() { MainValue = 3534, Time = DateTime.Now.AddMinutes(-15).AddDays(-1), Value2 = 17.2, Value3 = 0.016 },
                new SomeDataModel() { MainValue = 3512, Time = DateTime.Now.AddMinutes(-14).AddDays(-1), Value2 = 16.8, Value3 = 0.019 },
                new SomeDataModel() { MainValue = 3485, Time = DateTime.Now.AddMinutes(-13).AddDays(-1), Value2 = 16.5, Value3 = 0.015 },
                new SomeDataModel() { MainValue = 3485, Time = DateTime.Now.AddMinutes(-12).AddDays(-1), Value2 = 16.3, Value3 = 0.011 },
                new SomeDataModel() { MainValue = 3485, Time = DateTime.Now.AddMinutes(-11).AddDays(-1), Value2 = 15.6, Value3 = 0.009 },
                new SomeDataModel() { MainValue = 3485, Time = DateTime.Now.AddMinutes(-10).AddDays(-1), Value2 = 15.2, Value3 = 0.007 },
                new SomeDataModel() { MainValue = 3592, Time = DateTime.Now.AddMinutes(-9).AddDays(-1), Value2 = 14.7, Value3 = 0.005 },
                new SomeDataModel() { MainValue = 3591, Time = DateTime.Now.AddMinutes(-8).AddDays(-1), Value2 = 13.8, Value3 = 0.003 },
                new SomeDataModel() { MainValue = 3582, Time = DateTime.Now.AddMinutes(-7).AddDays(-1), Value2 = 14.2, Value3 = 0.001 },
                new SomeDataModel() { MainValue = 3543, Time = DateTime.Now.AddMinutes(-6).AddDays(-1), Value2 = 14.5, Value3 = -0.005 },
                new SomeDataModel() { MainValue = 3534, Time = DateTime.Now.AddMinutes(-5).AddDays(-1), Value2 = 14.8, Value3 = -0.011 },
                new SomeDataModel() { MainValue = 3512, Time = DateTime.Now.AddMinutes(-4).AddDays(-1), Value2 = 14.8, Value3 = -0.017 },
                new SomeDataModel() { MainValue = 3485, Time = DateTime.Now.AddMinutes(-3).AddDays(-1), Value2 = 15.2, Value3 = -0.019 },
                new SomeDataModel() { MainValue = 3611, Time = DateTime.Now.AddMinutes(-0).AddDays(-1), Value2 = 15.7, Value3 = -0.017 },
                new SomeDataModel() { MainValue = 3592, Time = DateTime.Now.AddMinutes(-29), Value2 = 17.7, Value3 = -0.013 },
                new SomeDataModel() { MainValue = 3591, Time = DateTime.Now.AddMinutes(-28), Value2 = 17.3, Value3 = -0.011 },
                new SomeDataModel() { MainValue = 3582, Time = DateTime.Now.AddMinutes(-27), Value2 = 16.9, Value3 = -0.008 },
                new SomeDataModel() { MainValue = 3543, Time = DateTime.Now.AddMinutes(-26), Value2 = 16.5, Value3 = -0.04 },
                new SomeDataModel() { MainValue = 3534, Time = DateTime.Now.AddMinutes(-25), Value2 = 16.4, Value3 = 0.001 },
                new SomeDataModel() { MainValue = 3512, Time = DateTime.Now.AddMinutes(-24), Value2 = 16.2, Value3 = 0.005 },
                new SomeDataModel() { MainValue = 3485, Time = DateTime.Now.AddMinutes(-23), Value2 = 15.9, Value3 = 0.011 },
                new SomeDataModel() { MainValue = 3584, Time = DateTime.Now.AddMinutes(-20), Value2 = 16.8, Value3 = 0.013 },
                new SomeDataModel() { MainValue = 3586, Time = DateTime.Now.AddMinutes(-19), Value2 = 18.4, Value3 = 0.017 },
                new SomeDataModel() { MainValue = 3534, Time = DateTime.Now.AddMinutes(-18), Value2 = 18.9, Value3 = 0.016 },
                new SomeDataModel() { MainValue = 3589, Time = DateTime.Now.AddMinutes(-17), Value2 = 19.1, Value3 = 0.021 },
                new SomeDataModel() { MainValue = 3612, Time = DateTime.Now.AddMinutes(-16), Value2 = 19.2, Value3 = 0.024 },
                new SomeDataModel() { MainValue = 3634, Time = DateTime.Now.AddMinutes(-15), Value2 = 19.5, Value3 = 0.025 },
                new SomeDataModel() { MainValue = 3639, Time = DateTime.Now.AddMinutes(-14), Value2 = 19.8, Value3 = 0.024 },
                new SomeDataModel() { MainValue = 3636, Time = DateTime.Now.AddMinutes(-13), Value2 = 19.5, Value3 = 0.023 },
                new SomeDataModel() { MainValue = 3628, Time = DateTime.Now.AddMinutes(-12), Value2 = 16.2, Value3 = 0.022 },
                new SomeDataModel() { MainValue = 3621, Time = DateTime.Now.AddMinutes(-11), Value2 = 15.5, Value3 = 0.020 },
                new SomeDataModel() { MainValue = 3611, Time = DateTime.Now.AddMinutes(-10), Value2 = 14.8, Value3 = 0.018 },
                new SomeDataModel() { MainValue = 3592, Time = DateTime.Now.AddMinutes(-9), Value2 = 13.5, Value3 = 0.015 },
                new SomeDataModel() { MainValue = 3591, Time = DateTime.Now.AddMinutes(-8), Value2 = 12.8, Value3 = 0.012 },
                new SomeDataModel() { MainValue = 3582, Time = DateTime.Now.AddMinutes(-7), Value2 = 11.8, Value3 = 0.011 },
                new SomeDataModel() { MainValue = 3543, Time = DateTime.Now.AddMinutes(-6), Value2 = 12.2, Value3 = 0.009 },
                new SomeDataModel() { MainValue = 3534, Time = DateTime.Now.AddMinutes(-5), Value2 = 11.8, Value3 = 0.012 },
                new SomeDataModel() { MainValue = 3512, Time = DateTime.Now.AddMinutes(-4), Value2 = 11.6, Value3 = 0.014 },
                new SomeDataModel() { MainValue = 3485, Time = DateTime.Now.AddMinutes(-3), Value2 = 11.2, Value3 = 0.018 },
                new SomeDataModel() { MainValue = 3485, Time = DateTime.Now.AddMinutes(-0), Value2 = 10.9, Value3 = 0.017 },
            };

            return xxx;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            int pixelX = (int)e.MouseDevice.GetPosition(wpfPlot1).X;
            int pixelY = (int)e.MouseDevice.GetPosition(wpfPlot1).Y;

            (double coordinateX, double coordinateY) = wpfPlot1.GetMouseCoordinates();

            XPixelLabel.Content = $"{pixelX:0.000}";
            YPixelLabel.Content = $"{pixelY:0.000}";

            //DateTime.FromOADate(pos).ToShortTimeString();
            var pos = wpfPlot1.Plot.GetCoordinateX(pixelX);
            //XCoordinateLabel.Content = DateTime.FromOADate(wpfPlot1.Plot.GetCoordinateX(pixelX)).ToShortTimeString();
            XCoordinateLabel.Content = pos > _positions.Last() ? _dataSetX.Last() : (pos < _positions.First() ? _dataSetX.First() : _dataSetX[(int)pos]);
            YCoordinateLabel.Content = $"{wpfPlot1.Plot.GetCoordinateY(pixelY):0.00000000}";
            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;
            wpfPlot1.Refresh();
        }

        private void wpfPlot1_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseTrackLabel.Content = "Mouse ENTERED the plot";
            Crosshair.IsVisible = true;
        }

        private void wpfPlot1_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseTrackLabel.Content = "Mouse LEFT the plot";
            XPixelLabel.Content = "--";
            YPixelLabel.Content = "--";
            XCoordinateLabel.Content = "--";
            YCoordinateLabel.Content = "--";

            Crosshair.IsVisible = false;
            wpfPlot1.Refresh();
        }
    }
}
