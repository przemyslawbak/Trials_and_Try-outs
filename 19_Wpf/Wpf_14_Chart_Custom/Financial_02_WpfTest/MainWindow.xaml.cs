using System.Windows;
using System.Windows.Media;

namespace Financial_02_WpfTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            AddCustomPiontsAndColor();
        }

        private void AddCustomPiontsAndColor()
        {
            Points.Add(new Point(1, 10));
            Points.Add(new Point(2, 30));
            Points.Add(new Point(3, 30));
            Points.Add(new Point(4, 20));
            Points.Add(new Point(5, 40));
            Points.Add(new Point(6, 70));
            Points.Add(new Point(7, 50));
            Points.Add(new Point(8, 10));
            Points.Add(new Point(9, 30));
            Points.Add(new Point(10, 30));
            Points.Add(new Point(11, 20));
            Points.Add(new Point(12, 40));
            Points.Add(new Point(13, 70));
            Points.Add(new Point(14, 50));
            Points.Add(new Point(15, 10));
            Points.Add(new Point(16, 30));
            Points.Add(new Point(17, 30));
            Points.Add(new Point(18, 20));
            Points.Add(new Point(19, 40));
            Points.Add(new Point(20, 70));
            Points.Add(new Point(21, 50));
            Points.Add(new Point(22, 10));
            Points.Add(new Point(23, 30));
            Points.Add(new Point(24, 30));
            Points.Add(new Point(25, 20));
            Points.Add(new Point(26, 40));
            Points.Add(new Point(27, 70));
            Points.Add(new Point(28, 50));
            Points.Add(new Point(29, 10));
            Points.Add(new Point(30, 30));
            Points.Add(new Point(31, 30));
            Points.Add(new Point(32, 20));
            Points.Add(new Point(33, 40));
            Points.Add(new Point(34, 70));
            Points.Add(new Point(35, 50));

            ColorName = "Red";
        }

        public PointCollection Points { get; set; } = new PointCollection();
        public string ColorName { get; set; }
    }
}
