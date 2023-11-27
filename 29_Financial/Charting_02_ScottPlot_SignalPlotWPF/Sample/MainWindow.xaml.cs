using ScottPlot.Drawing.Colormaps;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //https://scottplot.net/quickstart/wpf/
            //https://scottplot.net/cookbook/

            var dataSet1 = GetSomeData();

            WpfPlot1.Plot.AddSignal(dataSet1, sampleRate: 1);
            WpfPlot1.Plot.Title("Testing ScottPlot");
            WpfPlot1.Refresh();
        }

        private double[] GetSomeData()
        {
            var xxx =  new List<SomeDataModel>()
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

            return xxx.Select(x => x.Value).ToArray();
        }
    }
}
