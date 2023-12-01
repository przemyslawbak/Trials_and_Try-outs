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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int _labelSpacingDivider = 6;
        Crosshair Crosshair;
        string[] _dataSetX = Array.Empty<string>();
        double[] _dataSetY = Array.Empty<double>();
        double[] _positions = Array.Empty<double>();

        public MainWindow()
        {
            InitializeComponent();
            _dataSetY = GetSomeData().Select(x => x.Value).ToArray();
            _dataSetX = GetSomeData().Select(x => x.Time.ToString("dd-MM HH:mm")).ToArray();
            _positions = DataGen.Consecutive(_dataSetX.Length);
            wpfPlot1.Plot.AddSignalXY(_positions, _dataSetY);


            wpfPlot1.Plot.XAxis.DateTimeFormat(true);
            Crosshair = wpfPlot1.Plot.AddCrosshair(_positions[0], _dataSetY[0]);
            Crosshair.VerticalLine.PositionFormatter = pos => pos > _positions.Last() ? _dataSetX.Last() : (pos < _positions.First() ? _dataSetX.First() : _dataSetX[(int)pos]);
            
            wpfPlot1.Plot.XAxis.TickLabelStyle(rotation: 45);

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

        private List<SomeDataModel> GetSomeData()
        {
            var xxx = new List<SomeDataModel>()
            {
                new SomeDataModel() { Value = 3592, Time = DateTime.Now.AddMinutes(-19).AddDays(-1) },
                new SomeDataModel() { Value = 3591, Time = DateTime.Now.AddMinutes(-18).AddDays(-1) },
                new SomeDataModel() { Value = 3582, Time = DateTime.Now.AddMinutes(-17).AddDays(-1) },
                new SomeDataModel() { Value = 3543, Time = DateTime.Now.AddMinutes(-16).AddDays(-1) },
                new SomeDataModel() { Value = 3534, Time = DateTime.Now.AddMinutes(-15).AddDays(-1) },
                new SomeDataModel() { Value = 3512, Time = DateTime.Now.AddMinutes(-14).AddDays(-1) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-13).AddDays(-1) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-12).AddDays(-1) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-11).AddDays(-1) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-10).AddDays(-1) },
                new SomeDataModel() { Value = 3592, Time = DateTime.Now.AddMinutes(-9).AddDays(-1) },
                new SomeDataModel() { Value = 3591, Time = DateTime.Now.AddMinutes(-8).AddDays(-1) },
                new SomeDataModel() { Value = 3582, Time = DateTime.Now.AddMinutes(-7).AddDays(-1) },
                new SomeDataModel() { Value = 3543, Time = DateTime.Now.AddMinutes(-6).AddDays(-1) },
                new SomeDataModel() { Value = 3534, Time = DateTime.Now.AddMinutes(-5).AddDays(-1) },
                new SomeDataModel() { Value = 3512, Time = DateTime.Now.AddMinutes(-4).AddDays(-1) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-3).AddDays(-1) },
                new SomeDataModel() { Value = 3611, Time = DateTime.Now.AddMinutes(-0).AddDays(-1) },
                new SomeDataModel() { Value = 3592, Time = DateTime.Now.AddMinutes(-29) },
                new SomeDataModel() { Value = 3591, Time = DateTime.Now.AddMinutes(-28) },
                new SomeDataModel() { Value = 3582, Time = DateTime.Now.AddMinutes(-27) },
                new SomeDataModel() { Value = 3543, Time = DateTime.Now.AddMinutes(-26) },
                new SomeDataModel() { Value = 3534, Time = DateTime.Now.AddMinutes(-25) },
                new SomeDataModel() { Value = 3512, Time = DateTime.Now.AddMinutes(-24) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-23) },
                new SomeDataModel() { Value = 3584, Time = DateTime.Now.AddMinutes(-20) },
                new SomeDataModel() { Value = 3586, Time = DateTime.Now.AddMinutes(-19) },
                new SomeDataModel() { Value = 3534, Time = DateTime.Now.AddMinutes(-18) },
                new SomeDataModel() { Value = 3589, Time = DateTime.Now.AddMinutes(-17) },
                new SomeDataModel() { Value = 3612, Time = DateTime.Now.AddMinutes(-16) },
                new SomeDataModel() { Value = 3634, Time = DateTime.Now.AddMinutes(-15) },
                new SomeDataModel() { Value = 3639, Time = DateTime.Now.AddMinutes(-14) },
                new SomeDataModel() { Value = 3636, Time = DateTime.Now.AddMinutes(-13) },
                new SomeDataModel() { Value = 3628, Time = DateTime.Now.AddMinutes(-12) },
                new SomeDataModel() { Value = 3621, Time = DateTime.Now.AddMinutes(-11) },
                new SomeDataModel() { Value = 3611, Time = DateTime.Now.AddMinutes(-10) },
                new SomeDataModel() { Value = 3592, Time = DateTime.Now.AddMinutes(-9) },
                new SomeDataModel() { Value = 3591, Time = DateTime.Now.AddMinutes(-8) },
                new SomeDataModel() { Value = 3582, Time = DateTime.Now.AddMinutes(-7) },
                new SomeDataModel() { Value = 3543, Time = DateTime.Now.AddMinutes(-6) },
                new SomeDataModel() { Value = 3534, Time = DateTime.Now.AddMinutes(-5) },
                new SomeDataModel() { Value = 3512, Time = DateTime.Now.AddMinutes(-4) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-3) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-0) },
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
