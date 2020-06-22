using Financial.Commands;
using Financial.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;

namespace Financial.ViewModels
{
    public class PairStockData
    {
        public DateTime? Date { get; set; }
        public double? Price1 { get; set; }
        public double? Price2 { get; set; }

    }

    public class IndexData
    {
        public DateTime? Date { get; set; }
        public double? IGSpread { get; set; }
        public double? HYSpread { get; set; }
        public double? SPX { get; set; }
        public double? VIX { get; set; }
    }

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            StockData = new ObservableCollection<PairStockData>();
            StockReturnData = new ObservableCollection<PairStockData>();
            IndexData = new ObservableCollection<IndexData>();
            HySpxCollection = new ObservableCollection<Series>();
            HyVixCollection = new ObservableCollection<Series>();
            Stock1Collection = new ObservableCollection<Series>();
            Stock2Collection = new ObservableCollection<Series>();
        }

        public ObservableCollection<Series> HySpxCollection { get; set; }
        public ObservableCollection<Series> HyVixCollection { get; set; }
        public ObservableCollection<Series> Stock1Collection { get; set; }
        public ObservableCollection<Series> Stock2Collection { get; set; }
        public ObservableCollection<PairStockData> StockData { get; set; }
        public ObservableCollection<PairStockData> StockReturnData { get; set; }
        public ObservableCollection<IndexData> IndexData { get; set; }


        private string ticker = "GS";
        public string Ticker
        {
            get { return ticker; }
            set
            {
                ticker = value;
                OnPropertyChanged();
            }
        }

        private string title1 = string.Empty;
        public string Title1
        {
            get { return title1; }
            set
            {
                title1 = value;
                OnPropertyChanged();
            }
        }

        private string title2 = string.Empty;
        public string Title2
        {
            get { return title2; }
            set
            {
                title2 = value;
                OnPropertyChanged();
            }
        }

        private string title3 = string.Empty;
        public string Title3
        {
            get { return title3; }
            set
            {
                title3 = value;
                OnPropertyChanged();
            }
        }

        private string title4 = string.Empty;
        public string Title4
        {
            get { return title4; }
            set
            {
                title4 = value;
                OnPropertyChanged();
            }
        }

        //kurwa, czemu to łączy na sztywno?
        public void GetData()
        {
            IndexData.Clear();
            var idx = Dal.GetIndexData();
            DateTime startDate = ("5/15/2005").To<DateTime>();
            DateTime endDate = ("5/15/2014").To<DateTime>();
            IndexData.AddRange(Dal.GetIndexData(startDate, endDate));

            StockData.Clear();
            startDate = ("11/25/2013").To<DateTime>();
            endDate = ("11/25/2015").To<DateTime>();
            StockData.AddRange(Dal.GetPairStockData("^GSPC", Ticker, startDate, endDate, "Close", DataSourceEnum.Yahoo));

            StockReturnData.Clear();
            for (int i = 1; i < StockData.Count; i++)
            {
                var p0 = StockData[i - 1];
                var p1 = StockData[i];
                StockReturnData.Add(new PairStockData
                {
                    Date = p1.Date,
                    Price1 = (p1.Price1 - p0.Price1) / p0.Price1,
                    Price2 = (p1.Price2 - p0.Price2) / p0.Price2
                });
            }
        }

        public void PlotData()
        {
            IndexCharts();
            StockCharts();
        }

        private void IndexCharts()
        {
            HySpxCollection.Clear();
            SimpleLinearResult slr = LinearAnalysisHelper.GetSimpleRegression(IndexData, "SPX", "HYSpread");
            Title1 = string.Format("HY ~ SPX, a = {0}, b = {1}, R2 = {2}, R2Adj = {3} ", Math.Round(slr.Alpha, 4), Math.Round(slr.Beta, 4), Math.Round(slr.RSquared, 4), Math.Round(slr.RSquaredAdj, 4));
            HySpxCollection.AddRange(MSChartHelper.ScatterChart(IndexData, "SPX", "HYSpread", RegressionType.SimpleLinear));

            HyVixCollection.Clear();
            slr = LinearAnalysisHelper.GetSimpleRegression(IndexData, "VIX", "HYSpread");
            Title2 = string.Format("HY ~ VIX, a = {0}, b = {1}, R2 = {2}, R2Adj = {3}", Math.Round(slr.Alpha, 4), Math.Round(slr.Beta, 4), Math.Round(slr.RSquared, 4), Math.Round(slr.RSquaredAdj, 4));
            HyVixCollection.AddRange(MSChartHelper.ScatterChart(IndexData, "VIX", "HYSpread", RegressionType.SimpleLinear));
        }

        private void StockCharts()
        {
            Stock1Collection.Clear();
            SimpleLinearResult slr = LinearAnalysisHelper.GetSimpleRegression(StockData, "Price1", "Price2");
            Title3 = string.Format("Prices : {0} ~ SPX, a = {1}, b = {2}, R2 = {3}, R2Adj = {4}", Ticker, Math.Round(slr.Alpha, 4), Math.Round(slr.Beta, 4), Math.Round(slr.RSquared, 4), Math.Round(slr.RSquaredAdj, 4));
            Stock1Collection.AddRange(MSChartHelper.ScatterChart(StockData, "Price1", "Price2", RegressionType.SimpleLinear));

            Stock2Collection.Clear();
            slr = LinearAnalysisHelper.GetSimpleRegression(StockReturnData, "Price1", "Price2");
            Title4 = string.Format("Returns: {0} ~ SPX, a = {1}, b = {2}, R2 = {3}, R2Adj = {4} ", Ticker, Math.Round(slr.Alpha, 4), Math.Round(slr.Beta, 4), Math.Round(slr.RSquared, 4), Math.Round(slr.RSquaredAdj, 4));
            Stock2Collection.AddRange(MSChartHelper.ScatterChart(StockReturnData, "Price1", "Price2", RegressionType.SimpleLinear));
        }
    }

}
