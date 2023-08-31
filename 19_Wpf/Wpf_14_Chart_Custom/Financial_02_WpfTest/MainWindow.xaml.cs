using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
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
                //todo: how to get actual window height and width?
                //todo: fix lines row height
                //todo: process Value, depending on window height
                //todo: process X depending on window width

                Points.Add(new Point(i, data[i].Value));
            }

            ColorName = "Red";
        }

        public PointCollection Points { get; set; } = new PointCollection();
        public string ColorName { get; set; }
    }
}
