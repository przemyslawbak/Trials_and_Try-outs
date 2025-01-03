﻿using Bogus;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sample
{
    public partial class MainWindow : Window
    {
        int _labelSpacingDivider = 6;
        Crosshair Crosshair;
        string[] _dataSetX = Array.Empty<string>();
        double[] _positions = Array.Empty<double>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            var selectedViewModels = GetSelectedViewModels().Where(x => x.IsSelected);
            var selectedDataFromApi = GetDataFromApi(selectedViewModels).OrderBy(x => x.UtcTimeStamp);

            var mainDataType = "Index_SPXINDEX";
            var dataTypes = selectedDataFromApi.Where(x => x.DataType != mainDataType).Select(x => x.DataType).Distinct().ToArray();

            var _dataSetYMain = selectedDataFromApi.Where(x => x.DataType == mainDataType).Select(x => x.ResultValue).ToArray();
            _dataSetX = selectedDataFromApi.Where(x => x.DataType == mainDataType).Select(x => x.UtcTimeStamp.ToString("dd-MM HH:mm")).ToArray();
            _positions = DataGen.Consecutive(_dataSetX.Length);
            var valMain = wpfPlot.Plot.AddSignalXY(_positions, _dataSetYMain);
            wpfPlot.Plot.XAxis.DateTimeFormat(true);
            Crosshair = wpfPlot.Plot.AddCrosshair(_positions[0], _dataSetYMain[0]);
            Crosshair.VerticalLine.PositionFormatter = pos => pos > _positions.Last() ? _dataSetX.Last() : (pos < _positions.First() ? _dataSetX.First() : _dataSetX[(int)pos]);
            wpfPlot.Plot.XAxis.TickLabelStyle(rotation: 45);
            valMain.YAxisIndex = wpfPlot.Plot.LeftAxis.AxisIndex;
            wpfPlot.Plot.YAxis.Label("Index_SPXINDEX");
            wpfPlot.Plot.YAxis.Color(valMain.Color);
            CheckBoxes.Add(new CheckboxViewModel() { IsChecked = true, Name = mainDataType, Plot = valMain });

            foreach (var dataType in dataTypes)
            {
                var dataSetY = selectedDataFromApi.Where(x => x.DataType == dataType).Select(x => x.ResultValue).ToArray();

                var plt = wpfPlot.Plot.AddSignalXY(_positions, dataSetY);
                var yAxis = wpfPlot.Plot.AddAxis(ScottPlot.Renderable.Edge.Left);
                plt.YAxisIndex = yAxis.AxisIndex;
                yAxis.Label(dataType);
                yAxis.Color(plt.Color);

                plt.IsVisible = false;
                CheckBoxes.Add(new CheckboxViewModel() { IsChecked = plt.IsVisible, Name = dataType, Plot = plt });
            }

            wpfPlot.AxesChanged += OnAxesChanged;
            wpfPlot.Refresh();
        }

        public ObservableCollection<CheckboxViewModel> CheckBoxes { get; set; } = new ObservableCollection<CheckboxViewModel>();

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            foreach (var box in CheckBoxes)
            {
                box.Plot.IsVisible = box.IsChecked;
            };

            wpfPlot.Refresh();
        }

        private void OnAxesChanged(object sender, RoutedEventArgs e)
        {
            var ticksDisplayed = wpfPlot.Plot.XAxis.GetTicks().Length;
            var labelsSpacing = ticksDisplayed / _labelSpacingDivider;
            if (labelsSpacing != 0)
            {
                string[] labels = _dataSetX.Select((x, i) => i % labelsSpacing == 0 ? x : "").ToArray();
                wpfPlot.Plot.XAxis.ManualTickPositions(_positions, labels);
            }
            else
            {
                labelsSpacing = _dataSetX.Length / _labelSpacingDivider;
                string[] labels = _dataSetX.Select((x, i) => i % labelsSpacing == 0 ? x : "").ToArray();
                wpfPlot.Plot.XAxis.ManualTickPositions(_positions, labels);
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
            int pixelX = (int)e.MouseDevice.GetPosition(wpfPlot).X;
            int pixelY = (int)e.MouseDevice.GetPosition(wpfPlot).Y;

            (double coordinateX, double coordinateY) = wpfPlot.GetMouseCoordinates();

            XPixelLabel.Content = $"{pixelX:0.000}";
            YPixelLabel.Content = $"{pixelY:0.000}";

            //DateTime.FromOADate(pos).ToShortTimeString();
            var pos = wpfPlot.Plot.GetCoordinateX(pixelX);
            //XCoordinateLabel.Content = DateTime.FromOADate(wpfPlot1.Plot.GetCoordinateX(pixelX)).ToShortTimeString();
            XCoordinateLabel.Content = pos > _positions.Last() ? _dataSetX.Last() : (pos < _positions.First() ? _dataSetX.First() : _dataSetX[(int)pos]);
            YCoordinateLabel.Content = $"{wpfPlot.Plot.GetCoordinateY(pixelY):0.00000000}";
            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;
            wpfPlot.Refresh();
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
            wpfPlot.Refresh();
        }
    }
}
