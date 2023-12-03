using Bogus;
using Bogus.DataSets;
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
        double[] _positions = Array.Empty<double>();

        public MainWindow()
        {
            InitializeComponent();
            var selectedViewModels = GetSelectedViewModels().Where(x => x.IsSelected);
            var selectedDataFromApi = GetDataFromApi(selectedViewModels).OrderBy(x => x.UtcTimeStamp);

            var mainDataType = "Index_SPXINDEX";
            var dataTypes = selectedDataFromApi.Select(x => x.DataType).Distinct().ToArray();

            var _dataSetYMain = selectedDataFromApi.Where(x => x.DataType == mainDataType).Select(x => x.ResultValue).ToArray();
            _dataSetX = selectedDataFromApi.Where(x => x.DataType == mainDataType).Select(x => x.UtcTimeStamp.ToString("dd-MM HH:mm")).ToArray();
            _positions = DataGen.Consecutive(_dataSetX.Length);
            var valMain = wpfPlot1.Plot.AddSignalXY(_positions, _dataSetYMain);
            wpfPlot1.Plot.XAxis.DateTimeFormat(true);
            Crosshair = wpfPlot1.Plot.AddCrosshair(_positions[0], _dataSetYMain[0]);
            Crosshair.VerticalLine.PositionFormatter = pos => pos > _positions.Last() ? _dataSetX.Last() : (pos < _positions.First() ? _dataSetX.First() : _dataSetX[(int)pos]);
            wpfPlot1.Plot.XAxis.TickLabelStyle(rotation: 45);
            valMain.YAxisIndex = wpfPlot1.Plot.LeftAxis.AxisIndex;
            wpfPlot1.Plot.YAxis.Label("Main Value");
            wpfPlot1.Plot.YAxis.Color(valMain.Color);

            foreach (var dataType in dataTypes)
            {
                var dataSetY = selectedDataFromApi.Where(x => x.DataType == dataType).Select(x => x.ResultValue).ToArray();

                var val = wpfPlot1.Plot.AddSignalXY(_positions, dataSetY);
                var yAxis = wpfPlot1.Plot.AddAxis(ScottPlot.Renderable.Edge.Left);
                val.YAxisIndex = yAxis.AxisIndex;
                yAxis.Label("Value 2");
                yAxis.Color(val.Color);
            }

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

        private List<DataResultModel> GetDataFromApi(IEnumerable<DataTypeViewModel> selected)
        {
            var xxx = new List<DataResultModel>();
            var faker = new Faker();
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
                { "DataTypeC", true },
                { "DataTypeD", false },
            };

            for (int i = 0; i < selected.Count(); i++)
            {
                var resultValue = baseResultValueDictionary[selected.ToArray()[i].DataType];
                for (int j = 0; j < 100; j++)
                {
                    var randomValue = resultValue + faker.Random.Decimal(baseMinValRangeDictionary[selected.ToArray()[i].DataType], baseMaxValRangeDictionary[selected.ToArray()[i].DataType]);
                    xxx.Add(new DataResultModel()
                    {
                        DataGuid = Guid.NewGuid(),
                        DataType = selected.ToArray()[i].DataType,
                        IndexName = "SPX",
                        Release = releaseDictionary[selected.ToArray()[i].DataType],
                        ResultValue = (double)randomValue,
                        UtcTimeStamp = DateTime.UtcNow.AddMinutes(j),
                    });
                }
            }

            return xxx;
        }

        private IEnumerable<DataTypeViewModel> GetSelectedViewModels()
        {
            var xxx = new List<DataTypeViewModel>()
            {
                new DataTypeViewModel() { DataType = "Index_SPXINDEX", DataValue = "3485", Interval = 15, IsSelected = true, Multiplier = -1 },
                new DataTypeViewModel() { DataType = "DataTypeA", DataValue = "0.294", Interval = 15, IsSelected = false, Multiplier = 1 },
                new DataTypeViewModel() { DataType = "DataTypeB", DataValue = "10.9", Interval = 15, IsSelected = true, Multiplier = -1 },
                new DataTypeViewModel() { DataType = "DataTypeC", DataValue = "0.017", Interval = 15, IsSelected = true, Multiplier = 1 },
                new DataTypeViewModel() { DataType = "DataTypeD", DataValue = "183.15", Interval = 15, IsSelected = false, Multiplier = -1 },
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
