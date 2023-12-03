using ScottPlot.Plottable;

namespace Sample
{
    public class CheckboxViewModel
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public SignalPlotXY Plot { get; set; }
    }
}
