using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Financial_02_WpfTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            this.SizeChanged += OnWindowSizeChanged;
            chart_canvas.Width = this.Width - 140;
            chart_canvas.Height = this.Height/2;
            Color c = new Color() { ScA = 1, ScR = 1, ScG = 0, ScB = 0 };
            var line = new Line() { X1 = 0, Y1 = 20, X2 = chart_canvas.Width, Y2 = 20, Stroke = new SolidColorBrush(c), StrokeThickness = 2.0 };
            chart_canvas.Children.Add(line);
        }

        public double NewWindowHeight { get; set; } = 0;
        public double NewWindowWidth { get; set; } = 0;
        public PointCollection Points { get; set; } = new PointCollection();
        public string ColorName { get; set; }

        //https://stackoverflow.com/a/22870433/12603542
        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            NewWindowHeight = e.NewSize.Height;
            NewWindowWidth = e.NewSize.Width;
            Points.Clear();
            AddCustomPiontsAndColor();
        }

        private void AddCustomPiontsAndColor()
        {
            var data = new List<SomeDataModel>()
            {
                new SomeDataModel() { Value = 3584, Signal = 0 },
                new SomeDataModel() { Value = 3586, Signal = 0 },
                new SomeDataModel() { Value = 3534, Signal = 0 },
                new SomeDataModel() { Value = 3589, Signal = 0 },
                new SomeDataModel() { Value = 3612, Signal = 0 },
                new SomeDataModel() { Value = 3634, Signal = 1 },
                new SomeDataModel() { Value = 3639, Signal = 0 },
                new SomeDataModel() { Value = 3636, Signal = 0 },
                new SomeDataModel() { Value = 3628, Signal = 0 },
                new SomeDataModel() { Value = 3621, Signal = 0 },
                new SomeDataModel() { Value = 3611, Signal = 0 },
                new SomeDataModel() { Value = 3592, Signal = -1 },
                new SomeDataModel() { Value = 3591, Signal = 0 },
                new SomeDataModel() { Value = 3582, Signal = 0 },
                new SomeDataModel() { Value = 3543, Signal = 0 },
                new SomeDataModel() { Value = 3534, Signal = 0 },
                new SomeDataModel() { Value = 3512, Signal = 0 },
                new SomeDataModel() { Value = 3485, Signal = 0 },
            };

            for (int i = 0; i < data.Count; i++)
            {
                //todo: process Value, depending on window height
                //todo: process X depending on window width

                Point point = ComputePointCoordinates(data[i].Value, i, data);

                Points.Add(point);
            }

            ColorName = "Red";
        }

        private Point ComputePointCoordinates(int value, int i, List<SomeDataModel> data)
        {
            var maxVal = data.Max(d => d.Value);
            var minVal = data.Min(d => d.Value);
            var dataQty = data.Count;

            var containerHeight = NewWindowHeight / 2;
            var containerWidth = NewWindowWidth;

            var entityWidth = containerWidth / dataQty;
            var entityValue = 0; //todo: compute Y

            int x = (int)entityWidth * i;
            int y = 100; //todo: compute Y

            return new Point(x, y);
        }
    }
}
