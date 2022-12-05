using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Financial.Models
{
    public static class Dal
    {
        public static ObservableCollection<StockData> GetStockData(string ticker, DateTime startDate, DateTime endDate, DataSourceEnum dataSource)
        {
            ObservableCollection<StockData> res = new ObservableCollection<StockData>();

            switch (dataSource)
            {
                case DataSourceEnum.Yahoo:
                    res = GetStockDataFromYahoo(ticker, startDate, endDate);
                    break;
                case DataSourceEnum.Quandl:
                    res = GetStockDataFromQuandl(ticker, startDate, endDate);
                    break;
                case DataSourceEnum.Google:
                    res = GetStockDataFromGoogle(ticker, startDate, endDate);
                    break;
                case DataSourceEnum.Database:
                    res = GetStockDataFromDatabase(ticker, startDate, endDate);
                    break;
            }

            return res;
        }

        public static ObservableCollection<StockData> GetStockDataFromQuandl(string ticker, DateTime startDate, DateTime endDate)
        {
            ObservableCollection<StockData> res = new ObservableCollection<StockData>();
            DataTable dt = QuandlHelper.GetQuandlData(ticker, "WIKI", startDate, endDate);
            if (dt.Rows.Count < 1)
                return res;

            dt = ModelHelper.DatatableSort(dt, "Date ASC");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    res.Add(new StockData
                    {
                        Ticker = ticker,
                        Date = row["Date"].To<DateTime>(),
                        Open = row["Open"].To<double>(),
                        High = row["High"].To<double>(),
                        Low = row["Low"].To<double>(),
                        Close = row["Close"].To<double>(),
                        Volume = row["Volume"].To<double>(),
                        AdjOpen = row["Adj# Open"].To<double>(),
                        AdjHigh = row["Adj# High"].To<double>(),
                        AdjLow = row["Adj# Low"].To<double>(),
                        AdjClose = row["Adj# Close"].To<double>(),
                        AdjVolume = row["Volume"].To<double>()
                    });
                }
            }
            return res;
        }

        public static ObservableCollection<StockData> GetStockDataFromYahoo(string ticker, DateTime startDate, DateTime endDate)
        {
            ObservableCollection<StockData> res = new ObservableCollection<StockData>();
            DataTable dt = YahooHelper.GetYahooHistStockDataTable(ticker, startDate, endDate);
            if (dt.Rows.Count < 1)
                return res;
            dt = ModelHelper.DatatableSort(dt, "Date ASC");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    res.Add(new StockData
                    {
                        Ticker = ticker,
                        Date = row["Date"].To<DateTime>(),
                        Open = row["Open"].To<double>(),
                        High = row["High"].To<double>(),
                        Low = row["Low"].To<double>(),
                        Close = row["Close"].To<double>(),
                        AdjClose = row["Adj Close"].To<double>(),
                        Volume = row["Volume"].To<double>()
                    });
                }
            }
            return res;
        }

        public static ObservableCollection<StockData> GetStockDataFromGoogle(string ticker, DateTime startDate, DateTime endDate)
        {
            ObservableCollection<StockData> res = new ObservableCollection<StockData>();
            ObservableCollection<GoogleStockPrice> dt = GoogleHelper.GetGoogleStockData(ticker, startDate, endDate, GoogleDataTypeEnum.EOD);

            if (dt.Count > 0)
            {
                foreach (var p in dt)
                {
                    res.Add(new StockData
                    {
                        Ticker = ticker,
                        Date = p.Date.Date,
                        Open = p.PriceOpen.To<double>(),
                        High = p.PriceHigh.To<double>(),
                        Low = p.PriceLow.To<double>(),
                        Close = p.PriceClose.To<double>(),
                        Volume = p.Volume.To<double>()
                    });
                }
            }
            return res;
        }

        public static ObservableCollection<StockData> GetStockDataFromFile(string fileName, DateTime startDate, DateTime endDate)
        {
            ObservableCollection<StockData> res = new ObservableCollection<StockData>();
            var dt = ModelHelper.CsvToDatatable(fileName);
            string ticker = fileName.Substring(0, 6);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var date = Convert.ToDateTime(row["Date"]);
                    if (date >= startDate && date <= endDate)
                    {
                        res.Add(new StockData
                        {
                            Ticker = ticker,
                            Date = date,
                            Open = Convert.ToDouble(row["Open"]),
                            High = Convert.ToDouble(row["High"]),
                            Low = Convert.ToDouble(row["Low"]),
                            Close = Convert.ToDouble(row["Close"]),
                            Volume = Convert.ToDouble(row["Volume"])
                        });
                    }
                }
            }
            return res;
        }

        public static ObservableCollection<StockData> GetStockDataFromDatabase(string ticker, DateTime startDate, DateTime endDate)
        {
            ObservableCollection<StockData> res = new ObservableCollection<StockData>();
            using (var db = new MyDbEntities())
            {
                var query = (from s in db.Symbols
                             join p in db.Prices on s.SymbolID equals p.SymbolID
                             where s.Ticker == ticker && p.Date >= startDate && p.Date <= endDate
                             select new
                             {
                                 s.Ticker,
                                 p.Date,
                                 p.PriceOpen,
                                 p.PriceHigh,
                                 p.PriceLow,
                                 p.PriceClose,
                                 p.PriceAdj,
                                 p.Volume
                             }).OrderBy(x => x.Date);

                foreach (var p in query)
                {
                    res.Add(new StockData
                    {
                        Ticker = ticker,
                        Date = p.Date,
                        Open = p.PriceOpen,
                        High = p.PriceHigh,
                        Low = p.PriceLow,
                        Close = p.PriceClose,
                        AdjClose = p.PriceAdj,
                        Volume = p.Volume
                    });
                }
            }
            return res;
        }

        public static ObservableCollection<PairStockData> GetPairStockData(string ticker1, string ticker2, DateTime startDate, DateTime endDate, string priceType, DataSourceEnum dataSource)
        {
            ObservableCollection<PairStockData> res = new ObservableCollection<PairStockData>();
            DataTable dt = GetMultiStockData(new string[] { ticker1, ticker2 }, startDate, endDate, priceType, dataSource);
            if (dt.Rows.Count > 1)
            {
                foreach (DataRow row in dt.Rows)
                {
                    res.Add(new PairStockData
                    {
                        Date = row["Date"].To<DateTime>(),
                        Price1 = row[ticker1].To<double>(),
                        Price2 = row[ticker2].To<double>()
                    });
                }
            }
            return res;
        }

        public static BindableCollection<IndexData> GetIndexData()
        {
            ObservableCollection<IndexData> res = new ObservableCollection<IndexData>();
            DataTable dt = ModelHelper.CsvToDatatable(Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.FullName + @"\Models\DataModel\indices.csv");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    res.Add(new IndexData
                    {
                        Date = row["Date"].To<DateTime>(),
                        IGSpread = row["IGSpread"].To<double>(),
                        HYSpread = row["HYSpread"].To<double>(),
                        SPX = row["SPX"].To<double>(),
                        VIX = row["VIX"].To<double>(),
                    });
                }
            }
            return res;
        }

        public static ObservableCollection<IndexData> GetIndexData(DateTime startDate, DateTime endDate)
        {
            ObservableCollection<IndexData> res = new ObservableCollection<IndexData>();
            var data = GetIndexData();
            var query = from p in data where p.Date >= startDate && p.Date <= endDate select p;
            foreach (var p in query)
                res.Add(p);
            return res;
        }


        public static DataTable GetMultiStockData(string[] tickers, DateTime startDate, DateTime endDate, string priceType, DataSourceEnum dataSource)
        {
            DataTable res = new DataTable();

            ObservableCollection<StockData> dt = new ObservableCollection<StockData>();
            foreach (var ticker in tickers)
            {
                var tmp = GetStockData(ticker, startDate, endDate, dataSource);
                if (tmp.Count < 1)
                {
                    MessageBox.Show(string.Format("No data downloaded from {0} for Ticker = {1}. Please change data source or ticker...", dataSource, ticker));
                    return res;
                }
                dt.AddRange(GetStockData(ticker, startDate, endDate, dataSource));
            }

            DateTime date = startDate;
            int count = 0;
            while (date <= endDate)
            {
                var dts = from p in dt where p.Date == date orderby p.Ticker select p;
                if (dts.Count() == tickers.Length)
                {
                    if (count == 0)
                    {
                        res.Columns.Add("Date", typeof(DateTime));
                        foreach (var r in dts)
                            res.Columns.Add(r.Ticker, typeof(double));
                    }
                    res.Rows.Add(date);

                    int n = 0;
                    foreach (var r in dts)
                    {
                        res.Rows[res.Rows.Count - 1][n + 1] = r.GetType().GetProperty(priceType).GetValue(r, null);
                        n++;
                    }
                    count++;
                }
                date = date.AddDays(1);
            }
            return res;
        }
    }


    public class StockData
    {
        public string Ticker { get; set; }
        public DateTime? Date { get; set; }
        public double? Open { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public double? Close { get; set; }
        public double? Volume { get; set; }
        public double? AdjOpen { get; set; }
        public double? AdjHigh { get; set; }
        public double? AdjLow { get; set; }
        public double? AdjClose { get; set; }
        public double? AdjVolume { get; set; }
    }

    public class IndexData
    {
        public DateTime? Date { get; set; }
        public double? IGSpread { get; set; }
        public double? HYSpread { get; set; }
        public double? SPX { get; set; }
        public double? VIX { get; set; }
    }

    public class PairStockData
    {
        public DateTime? Date { get; set; }
        public double? Price1 { get; set; }
        public double? Price2 { get; set; }

    }

    public enum DataSourceEnum
    {
        Yahoo = 0,
        Quandl = 1,
        Google = 2,
        Database = 3,
    }
}
