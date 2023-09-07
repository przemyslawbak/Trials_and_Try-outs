using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        }

        public string ColorName { get; set; }
        public int XPrev { get; set; } = 0;
        public int YPrev { get; set; } = 100;


        protected async void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            XPrev = 0;
            YPrev = 100;
            chart_canvas.Width = e.NewSize.Width - 140;
            chart_canvas.Height = e.NewSize.Height / 2.5;
            chart_canvas.Children.Clear();

            await AddCustomPiontsAndColorAsync();

            //todo: https://stackoverflow.com/questions/4253088/updating-gui-wpf-using-a-different-thread
        }

        public void TimeConsumingBlockingUi(int n)
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
        }

        private async Task AddCustomPiontsAndColorAsync()
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

            var coordinatesList = new List<CoordinatesModel>();

            for (int i = 0; i < data.Count; i++)
            {
                var width = chart_canvas.Width;
                var height = chart_canvas.Height;

                var coordinates = await Task.Run(() => ComputePointCoordinates(data[i].Value, i, data, width, height));
                coordinatesList.Add(coordinates);

            }

            var resultCoordinates = coordinatesList.OrderBy(x => x.X).ToList();

            chart_canvas.Children.Clear();

            foreach (var coordinate in resultCoordinates)
            {
                Color c = new Color() { ScA = 1, ScR = 1, ScG = 0, ScB = 0 };
                var line = new Line() { X1 = XPrev, Y1 = YPrev, X2 = coordinate.X, Y2 = coordinate.Y, Stroke = new SolidColorBrush(c), StrokeThickness = 1.0 };
                chart_canvas.Children.Add(line);
                XPrev = coordinate.X;
                YPrev = coordinate.Y;
            }

            ColorName = "Red";
        }

        private CoordinatesModel ComputePointCoordinates(int value, int i, List<SomeDataModel> data, double width, double height)
        {
            TimeConsumingBlockingUi(10000);

            var coordinates = new CoordinatesModel();
            var maxVal = data.Max(d => d.Value);
            var minVal = data.Min(d => d.Value);
            var valDiff = maxVal - minVal;


            var dataQty = data.Count;

            var entityWidth = width / dataQty;

            coordinates.X = (int)entityWidth * i;
            if (valDiff > height)
            {
                var change = valDiff - height;
                coordinates.Y = value - minVal - (int)change;
            }
            else
            {
                coordinates.Y = value - minVal;
            }

            return coordinates;
        }
    }
}
