using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Financial_02_WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyViewModel vm = new MyViewModel();
            _myPolyline.Points = vm.myModel.points;
        }
    }

    public class MyViewModel
    {

        public int currentSecond = 0;
        Random rd = new Random();
        public PointCollection LtPoint = new PointCollection();

        public MyModel myModel { get; set; } = new MyModel();
        public MyViewModel()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Start();

            myModel = new MyModel()
            {
                points = LtPoint,
                ColorName = "Blue"
            };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentSecond++;
            double x = currentSecond * 10;
            double y = rd.Next(1, 200);
            LtPoint.Add(new Point(x, y));
        }
    }

    public class MyModel
    {
        public PointCollection points { get; set; } = new PointCollection();

        public string ColorName { get; set; }
    }
}
