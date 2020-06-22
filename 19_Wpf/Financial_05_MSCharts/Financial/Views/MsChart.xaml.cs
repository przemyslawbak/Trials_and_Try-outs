using ChartControl;
using Financial.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;

namespace Financial.Views
{
    /// <summary>
    /// Interaction logic for MsChart.xaml
    /// </summary>
    public partial class MsChart : UserControl
    {
        public MsChart()
        {
            InitializeComponent();
            SeriesCollection = new ObservableCollection<Series>();
            Periods = new ObservableCollection<int>();
        }

        public static DependencyProperty XValueTypeProperty = DependencyProperty.Register("XValueType", typeof(string),
            typeof(MsChart), new FrameworkPropertyMetadata("Double", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string XValueType
        {
            get { return (string)GetValue(XValueTypeProperty); }
            set { SetValue(XValueTypeProperty, value); }
        }

        public static DependencyProperty XLabelProperty = DependencyProperty.Register("XLabel", typeof(string),
            typeof(MsChart), new FrameworkPropertyMetadata("X Axis", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string XLabel
        {
            get { return (string)GetValue(XLabelProperty); }
            set { SetValue(XLabelProperty, value); }
        }

        public static DependencyProperty YLabelProperty = DependencyProperty.Register("YLabel", typeof(string),
            typeof(MsChart), new FrameworkPropertyMetadata("Y Axis", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string YLabel
        {
            get { return (string)GetValue(YLabelProperty); }
            set { SetValue(YLabelProperty, value); }
        }

        public static DependencyProperty Y2LabelProperty = DependencyProperty.Register("Y2Label", typeof(string),
           typeof(MsChart), new FrameworkPropertyMetadata("Y2 Axis", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Y2Label
        {
            get { return (string)GetValue(Y2LabelProperty); }
            set { SetValue(Y2LabelProperty, value); }
        }

        public static DependencyProperty IsArea3DProperty = DependencyProperty.Register("IsArea3D", typeof(bool),
        typeof(MsChart), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsArea3D
        {
            get { return (bool)GetValue(IsArea3DProperty); }
            set { SetValue(IsArea3DProperty, value); }
        }

        /*public static DependencyProperty Y2ColumnNamesProperty = DependencyProperty.Register("Y2ColumnNames", typeof(List<string>),
         typeof(MsChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public List<string> Y2ColumnNames
        {
            get { return (List<string>)GetValue(Y2ColumnNamesProperty); }
            set { SetValue(Y2ColumnNamesProperty, value); }
        }*/

        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string),
            typeof(MsChart), new FrameworkPropertyMetadata("My Title", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static DependencyProperty ChartTypeProperty = DependencyProperty.Register("ChartType", typeof(ChartTypeEnum),
           typeof(MsChart), new FrameworkPropertyMetadata(ChartTypeEnum.MyChart, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ChartTypeEnum ChartType
        {
            get { return (ChartTypeEnum)GetValue(ChartTypeProperty); }
            set { SetValue(ChartTypeProperty, value); }
        }

        public static DependencyProperty ChartBackgroundProperty = DependencyProperty.Register("ChartBackground", typeof(ChartBackgroundColor),
           typeof(MsChart), new FrameworkPropertyMetadata(ChartBackgroundColor.Blue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ChartBackgroundColor ChartBackground
        {
            get { return (ChartBackgroundColor)GetValue(ChartBackgroundProperty); }
            set { SetValue(ChartBackgroundProperty, value); }
        }


        public static readonly DependencyProperty SeriesCollectionProperty = DependencyProperty.Register("SeriesCollection",
                typeof(ObservableCollection<Series>), typeof(MsChart), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSeriesChanged));

        public ObservableCollection<Series> SeriesCollection
        {
            get { return (ObservableCollection<Series>)GetValue(SeriesCollectionProperty); }
            set { SetValue(SeriesCollectionProperty, value); }
        }

        private static void OnSeriesChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ms = sender as MsChart;
            var sc = e.NewValue as ObservableCollection<Series>;
            if (sc != null)
                sc.CollectionChanged += ms.sc_CollectionChanged;
        }

        private void sc_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SeriesCollection != null)
            {
                CheckCount = 0;
                if (SeriesCollection.Count > 0)
                    CheckCount = SeriesCollection.Count;
            }
        }


        private static DependencyProperty CheckCountProperty = DependencyProperty.Register("CheckCount", typeof(int),
            typeof(MsChart), new FrameworkPropertyMetadata(0, StartChart));

        private int CheckCount
        {
            get { return (int)GetValue(CheckCountProperty); }
            set { SetValue(CheckCountProperty, value); }
        }

        private static void StartChart(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ms = sender as MsChart;

            if (ms.CheckCount > 0)
            {
                ms.myChart.Visible = true;
                ms.myChart.Series.Clear();
                ms.myChart.Titles.Clear();
                ms.myChart.Legends.Clear();
                ms.myChart.ChartAreas.Clear();
                if (ms.ChartType == ChartTypeEnum.MyChart)
                {
                    MSChartHelper.MyChart(ms.myChart, ms.SeriesCollection, ms.Title, ms.XLabel, ms.YLabel, ms.ChartBackground, ms.Y2Label);
                    if (ms.myChart.ChartAreas.Count > 0)
                        ms.myChart.ChartAreas[0].Area3DStyle.Enable3D = ms.IsArea3D;
                }
                else if (ms.ChartType == ChartTypeEnum.MyChart2)
                {
                    MSChartHelper.MyChart2(ms.myChart, ms.SeriesCollection, ms.Title, ms.XLabel, ms.YLabel, ms.ChartBackground, ms.Y2Label);
                }
                else if (ms.ChartType == ChartTypeEnum.MyChart3)
                {
                    MSChartHelper.MyChart3(ms.myChart, ms.SeriesCollection, ms.Title, ms.XLabel, ms.YLabel, ms.ChartBackground, ms.Y2Label);
                }
                ms.myChart.DataBind();
            }
            else
                ms.myChart.Visible = false;
        }

        public static DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(object),
          typeof(MsChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));

        public object DataSource
        {
            get { return (object)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        private static void OnSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ms = sender as MsChart;
            var sc = e.NewValue as object;
            if (sc != null)
            {
                ms.myChart.DataSource = ms.DataSource;
            }
        }


        public static DependencyProperty Chart1Property = DependencyProperty.Register("Chart1", typeof(Chart),
         typeof(MsChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChartChanged));

        public Chart Chart1
        {
            get { return (Chart)GetValue(Chart1Property); }
            set { SetValue(Chart1Property, value); }
        }

        private static void OnChartChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ms = sender as MsChart;
            var sc = e.NewValue as Chart;
            if (sc != null)
            {
                ms.myChart = ms.Chart1;
                ChartArea area = new ChartArea();
                MSChartHelper.ChartStyle(ms.Chart1, area, ChartBackgroundColor.Blue);
            }
        }








        public static DependencyProperty OutputTableProperty = DependencyProperty.Register("OutputTable", typeof(DataTable),
         typeof(MsChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DataTable OutputTable
        {
            get { return (DataTable)GetValue(OutputTableProperty); }
            set { SetValue(OutputTableProperty, value); }
        }

        public static DependencyProperty StockPricesProperty = DependencyProperty.Register("StockPrices", typeof(ObservableCollection<StockPrice>),
          typeof(MsChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<StockPrice> StockPrices
        {
            get { return (ObservableCollection<StockPrice>)GetValue(StockPricesProperty); }
            set { SetValue(StockPricesProperty, value); }
        }

        public static DependencyProperty PeriodsProperty = DependencyProperty.Register("Periods", typeof(ObservableCollection<int>),
          typeof(MsChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<int> Periods
        {
            get { return (ObservableCollection<int>)GetValue(PeriodsProperty); }
            set { SetValue(PeriodsProperty, value); }
        }

        public static DependencyProperty FinancialFormulaProperty = DependencyProperty.Register("FinancialFormula", typeof(FinancialFormula),
          typeof(MsChart), new FrameworkPropertyMetadata(FinancialFormula.WilliamsR, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public FinancialFormula FinancialFormula
        {
            get { return (FinancialFormula)GetValue(FinancialFormulaProperty); }
            set { SetValue(FinancialFormulaProperty, value); }
        }


        public static DependencyProperty IsStartIndicatorProperty = DependencyProperty.Register("IsStartIndicator", typeof(bool),
          typeof(MsChart), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIndicatorChanged));

        public bool IsStartIndicator
        {
            get { return (bool)GetValue(IsStartIndicatorProperty); }
            set { SetValue(IsStartIndicatorProperty, value); }
        }

        private static void OnIndicatorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ms = sender as MsChart;
            var sc = (bool)e.NewValue;
            if (sc)
            {
                ms.myChart.Series.Clear();
                ms.myChart.Titles.Clear();
                ms.myChart.Legends.Clear();
                ms.myChart.ChartAreas.Clear();
                if (ms.FinancialFormula == FinancialFormula.Forecasting)
                {
                    MessageBox.Show("This indicator is not implemented...");
                    return;
                }
                ms.OutputTable = MSChartHelper.IndicatorChart(ms.myChart, ms.StockPrices, ms.FinancialFormula, ms.Periods, ms.ChartBackground);
            }
        }


        public static DependencyProperty IsStartMyIndicatorProperty = DependencyProperty.Register("IsStartMyIndicator", typeof(bool),
         typeof(MsChart), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMyIndicatorChanged));

        public bool IsStartMyIndicator
        {
            get { return (bool)GetValue(IsStartMyIndicatorProperty); }
            set { SetValue(IsStartMyIndicatorProperty, value); }
        }

        private static void OnMyIndicatorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ms = sender as MsChart;
            var sc = (bool)e.NewValue;
            if (sc)
            {
                ms.myChart.Series.Clear();
                ms.myChart.Titles.Clear();
                ms.myChart.Legends.Clear();
                ms.myChart.ChartAreas.Clear();
                if (ms.FinancialFormula == FinancialFormula.Forecasting)
                {
                    MessageBox.Show("This indicator is not implemented...");
                    return;
                }
                ms.OutputTable = MSChartHelper.IndicatorChart(ms.myChart, ms.StockPrices, ms.FinancialFormula, ms.Periods, ms.ChartBackground);
            }
        }
    }
}
