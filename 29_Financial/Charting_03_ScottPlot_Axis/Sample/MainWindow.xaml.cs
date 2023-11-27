using System;
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

            var dataSet1 = GetSomeData().Select(x => x.Value).ToArray();
            var dataSet2 = GetSomeData().Select(x => x.Time.ToOADate()).ToArray();

            WpfPlot1.Plot.AddScatter(dataSet2, dataSet1);
            WpfPlot1.Plot.Title("Testing ScottPlot with DateTime");
            WpfPlot1.Plot.XAxis.DateTimeFormat(true);

            WpfPlot1.Refresh();
        }

        private List<SomeDataModel> GetSomeData()
        {
            var xxx =  new List<SomeDataModel>()
            {
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
            };

            return xxx;
        }
    }
}
