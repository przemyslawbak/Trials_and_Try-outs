using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        }

        public double NewWindowHeight { get; set; } = 0;
        public double NewWindowWidth { get; set; } = 0;
        public string ColorName { get; set; }
        public int XPrev { get; set; } = 0;
        public int YPrev { get; set; } = 100;

        //https://stackoverflow.com/a/22870433/12603542
        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            XPrev = 0;
            YPrev = 100;
            chart_canvas.Width = e.NewSize.Width - 140;
            chart_canvas.Height = e.NewSize.Height / 2.5;
            chart_canvas.Children.Clear();
            AddCustomPiontsAndColor();
        }

        public long TimeConsumingBlockingUi(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }

        private void AddCustomPiontsAndColor()
        {
            long nthPrime = TimeConsumingBlockingUi(200000);

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
                ComputePointCoordinates(data[i].Value, i, data);
            }

            ColorName = "Red";
        }

        private void ComputePointCoordinates(int value, int i, List<SomeDataModel> data)
        {
            var maxVal = data.Max(d => d.Value);
            var minVal = data.Min(d => d.Value);
            var valDiff = maxVal - minVal;


            var dataQty = data.Count;

            var entityWidth = chart_canvas.Width / dataQty;

            int y = 0;
            int x = (int)entityWidth * i;
            if (valDiff > chart_canvas.Height)
            {
                var change = valDiff - chart_canvas.Height;
                y = value - minVal - (int)change;
            }
            else
            {
                y = value - minVal;
            }

            Color c = new Color() { ScA = 1, ScR = 1, ScG = 0, ScB = 0 };
            var line = new Line() { X1 = XPrev, Y1 = YPrev, X2 = x, Y2 = y, Stroke = new SolidColorBrush(c), StrokeThickness = 1.0 };
            chart_canvas.Children.Add(line);
            XPrev = x;
            YPrev = y;
        }
    }
}
