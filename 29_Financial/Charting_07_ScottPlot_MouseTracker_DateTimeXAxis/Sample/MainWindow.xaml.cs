﻿using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
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
        Crosshair Crosshair;

        public MainWindow() //https://scottplot.net/faq/datetime/
        {
            InitializeComponent();
            var dataSetY = GetSomeData().Select(x => x.Value).ToArray();
            var dataSetX = GetSomeData().Select(x => x.Time.ToOADate()).ToArray();

            wpfPlot1.Plot.AddSignalXY(dataSetX, dataSetY);
            wpfPlot1.Plot.XAxis.DateTimeFormat(true);
            Crosshair = wpfPlot1.Plot.AddCrosshair(dataSetX[0], dataSetY[0]);
            wpfPlot1.Refresh();
        }
        private List<SomeDataModel> GetSomeData()
        {
            var xxx = new List<SomeDataModel>()
            {
                new SomeDataModel() { Value = 3592, Time = DateTime.Now.AddMinutes(-49) },
                new SomeDataModel() { Value = 3591, Time = DateTime.Now.AddMinutes(-48) },
                new SomeDataModel() { Value = 3582, Time = DateTime.Now.AddMinutes(-47) },
                new SomeDataModel() { Value = 3543, Time = DateTime.Now.AddMinutes(-46) },
                new SomeDataModel() { Value = 3534, Time = DateTime.Now.AddMinutes(-45) },
                new SomeDataModel() { Value = 3512, Time = DateTime.Now.AddMinutes(-44) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-43) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-42) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-41) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-40) },
                new SomeDataModel() { Value = 3592, Time = DateTime.Now.AddMinutes(-39) },
                new SomeDataModel() { Value = 3591, Time = DateTime.Now.AddMinutes(-38) },
                new SomeDataModel() { Value = 3582, Time = DateTime.Now.AddMinutes(-37) },
                new SomeDataModel() { Value = 3543, Time = DateTime.Now.AddMinutes(-36) },
                new SomeDataModel() { Value = 3534, Time = DateTime.Now.AddMinutes(-35) },
                new SomeDataModel() { Value = 3512, Time = DateTime.Now.AddMinutes(-34) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-33) },
                new SomeDataModel() { Value = 3611, Time = DateTime.Now.AddMinutes(-30) },
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
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-2) },
                new SomeDataModel() { Value = 3485, Time = DateTime.Now.AddMinutes(-1) },
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

            XCoordinateLabel.Content = $"{wpfPlot1.Plot.GetCoordinateX(pixelX):0.00000000}";
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
