using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Financial.Models
{
    public static class YahooHelper
    {
        //private const string url_hist = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.historicaldata%20where%20symbol%20in%20({0})%20and%20startDate%20%3D%20%22{1}%22%20and%20endDate%20%3D%20%22{2}%22&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
        private const string url_quotes = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20({0})&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";


        /*public static DataTable GetHistPricesTable(List<YahooTicker> tickers, DateTime startDate, DateTime endDate)
        {
            DataTable res = new DataTable();
            string tickerList = string.Join("%2C", tickers.Select(x => "%22" + x.Ticker + "%22").ToArray());
            string url = string.Format(url_hist, tickerList, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            XDocument doc = XDocument.Load(url);
            XElement results = doc.Root.Element("results");
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(results.ToString()));
            if(ds!=null)
            {
                if (ds.Tables.Count > 0)
                    res = ds.Tables[0];
            }
            return res;
        }

        public static BindableCollection<StockPrice> GetHistPrices(List<YahooTicker> tickers, DateTime startDate, DateTime endDate)
        {
            var dt = GetHistPricesTable(tickers, startDate, endDate);
            var prices = new BindableCollection<StockPrice>();
            foreach (DataRow r in dt.Rows)
            {
                prices.Add(new StockPrice("")
                {
                    Ticker = r["Symbol"].ToString(),
                    Date = GetDateTime(r["Date"].ToString()),
                    PriceOpen = GetDecimal(r["Open"].ToString()),
                    PriceHigh = GetDecimal(r["High"].ToString()),
                    PriceLow = GetDecimal(r["Low"].ToString()),
                    PriceClose = GetDecimal(r["Close"].ToString()),
                    PriceAdj = GetDecimal(r["Adj_Close"].ToString()),
                    Volume = GetDecimal(r["Volume"].ToString())
                });
            }
            return prices;
        }*/


        public static DataTable GetYahooHistStockDataTable1(string ticker, DateTime? startDate, DateTime? endDate)
        {
            string url = @"http://ichart.finance.yahoo.com/table.csv?s=[symbol]&a=[startMonth]&b=[startDay]&c=[startYear]&d=[endMonth]&e=[endDay]&f=[endYear]&g=d&ignore=.csv";
            if (!endDate.HasValue) endDate = DateTime.Now;
            if (!startDate.HasValue) startDate = DateTime.Now.AddYears(-5);
            if (ticker == null || ticker.Length < 1)
                throw new ArgumentException("Symbol invalid: " + ticker);

            // NOTE: Yahoo's scheme uses a month number 1 less than actual e.g. Jan. ="0"
            int strtMo = startDate.Value.Month - 1;
            string startMonth = strtMo.ToString();
            string startDay = startDate.Value.Day.ToString();
            string startYear = startDate.Value.Year.ToString();

            int endMo = endDate.Value.Month - 1;
            string endMonth = endMo.ToString();
            string endDay = endDate.Value.Day.ToString();
            string endYear = endDate.Value.Year.ToString();

            url = url.Replace("[symbol]", ticker);
            url = url.Replace("[startMonth]", startMonth);
            url = url.Replace("[startDay]", startDay);
            url = url.Replace("[startYear]", startYear);
            url = url.Replace("[endMonth]", endMonth);
            url = url.Replace("[endDay]", endDay);
            url = url.Replace("[endYear]", endYear);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    if (File.Exists("myYahoo.csv"))
                        File.Delete("myYahoo.csv");
                    wc.DownloadFile(url, "myYahoo.csv");
                }
                catch { }
            }

            DataTable dt = new DataTable();
            if (File.Exists("myYahoo.csv"))
            {
                dt = ModelHelper.CsvToDatatable("myYahoo.csv");
                File.Delete("myYahoo.csv");
            }

            dt.Columns.Add("Ticker", typeof(string));
            foreach (DataRow row in dt.Rows)
                row["Ticker"] = ticker;
            return dt;
        }

        public static DataTable GetYahooHistStockDataTable(string ticker, DateTime? startDate, DateTime? endDate)
        {

            string urlTemplate =
              @"http://ichart.finance.yahoo.com/table.csv?s=[symbol]&a=[startMonth]&b=[startDay]&c=[startYear]&d=[endMonth]&e=[endDay]&f=[endYear]&g=d&ignore=.csv";
            if (!endDate.HasValue) endDate = DateTime.Now;
            if (!startDate.HasValue) startDate = DateTime.Now.AddYears(-5);
            if (ticker == null || ticker.Length < 1)
                throw new ArgumentException("Symbol invalid: " + ticker);

            // NOTE: Yahoo's scheme uses a month number 1 less than actual e.g. Jan. ="0"
            int strtMo = startDate.Value.Month - 1;
            string startMonth = strtMo.ToString();
            string startDay = startDate.Value.Day.ToString();
            string startYear = startDate.Value.Year.ToString();

            int endMo = endDate.Value.Month - 1;
            string endMonth = endMo.ToString();
            string endDay = endDate.Value.Day.ToString();
            string endYear = endDate.Value.Year.ToString();

            urlTemplate = urlTemplate.Replace("[symbol]", ticker);
            urlTemplate = urlTemplate.Replace("[startMonth]", startMonth);
            urlTemplate = urlTemplate.Replace("[startDay]", startDay);
            urlTemplate = urlTemplate.Replace("[startYear]", startYear);
            urlTemplate = urlTemplate.Replace("[endMonth]", endMonth);
            urlTemplate = urlTemplate.Replace("[endDay]", endDay);
            urlTemplate = urlTemplate.Replace("[endYear]", endYear);
            string history = String.Empty;

            //MessageBox.Show(urlTemplate);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    history = wc.DownloadString(urlTemplate);
                }
                catch { }
            }

            DataTable dt = new DataTable();
            // trim off unused characters from end of line
            history = history.Replace("\r", "");
            // split to array on end of line
            string[] rows = history.Split('\n');
            // split to colums
            string[] colNames = rows[0].Split(',');
            // add the columns to the DataTable
            foreach (string colName in colNames)
                dt.Columns.Add(colName);
            DataRow row = null;
            string[] rowValues;
            object[] rowItems;
            // split the rows
            for (int i = rows.Length - 1; i > 0; i--)
            {
                rowValues = rows[i].Split(',');
                row = dt.NewRow();
                rowItems = ConvertStringArrayToObjectArray(rowValues);
                if (rowItems[0] != null && (string)rowItems[0] != "")
                {
                    row.ItemArray = rowItems;
                    dt.Rows.Add(row);
                }
            }
            //return dt;

            DataTable res = new DataTable();
            res.Columns.Add("Ticker", typeof(string));
            res.Columns.Add("Date", typeof(DateTime));
            res.Columns.Add("Open", typeof(decimal));
            res.Columns.Add("High", typeof(decimal));
            res.Columns.Add("Low", typeof(decimal));
            res.Columns.Add("Close", typeof(decimal));
            res.Columns.Add("Volume", typeof(decimal));
            res.Columns.Add("Adj Close", typeof(decimal));

            foreach (DataRow row1 in dt.Rows)
            {
                DateTime date = (DateTime.ParseExact(row1["Date"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToLocalTime()).AddDays(1);
                date = Convert.ToDateTime(date.ToShortDateString());
                res.Rows.Add(ticker, date,
                    Convert.ToDecimal(row1["Open"]),
                    Convert.ToDecimal(row1["High"]),
                    Convert.ToDecimal(row1["Low"]),
                    Convert.ToDecimal(row1["Close"]),
                    Convert.ToDecimal(row1["Volume"]),
                    Convert.ToDecimal(row1["Adj Close"]));
            }

            return res;
        }

        private static object[] ConvertStringArrayToObjectArray(string[] input)
        {
            int elements = input.Length;
            object[] objArray = new object[elements];
            input.CopyTo(objArray, 0);
            return objArray;
        }


        public static DataTable GetQuotesTable(ObservableCollection<StockQuote> stockQuotes)
        {
            string symbolList = string.Join("%2C", stockQuotes.Select(x => "%22" + x.Symbol + "%22").ToArray());
            string url = string.Format(url_quotes, symbolList);
            XDocument doc = XDocument.Load(url);
            XElement results = doc.Root.Element("results");
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(results.ToString()));
            return ds.Tables[0];
        }

        public static void GetQuotes(ObservableCollection<StockQuote> stockQuotes)
        {
            string symbolList = string.Join("%2C", stockQuotes.Select(x => "%22" + x.Symbol + "%22").ToArray());
            string url = string.Format(url_quotes, symbolList);
            XDocument doc = XDocument.Load(url);
            ParseQuotes(stockQuotes, doc);
        }

        private static void ParseQuotes(ObservableCollection<StockQuote> quotes, XDocument doc)
        {
            XElement results = doc.Root.Element("results");
            foreach (StockQuote quote in quotes)
            {
                try
                {
                    XElement element = results.Elements("quote").First(x => x.Attribute("symbol").Value == quote.Symbol);
                    quote.Ask = GetDecimal(element.Element("Ask").Value);
                    quote.Bid = GetDecimal(element.Element("Bid").Value);
                    quote.Change = GetDecimal(element.Element("Change").Value);
                    quote.PercentChange = GetDecimal(element.Element("PercentChange").Value);
                    quote.LastTradeTime = GetDateTime(element.Element("LastTradeTime").Value);
                    quote.DailyLow = GetDecimal(element.Element("DaysLow").Value);
                    quote.DailyHigh = GetDecimal(element.Element("DaysHigh").Value);
                    quote.YearlyLow = GetDecimal(element.Element("YearLow").Value);
                    quote.YearlyHigh = GetDecimal(element.Element("YearHigh").Value);
                    quote.LastTradePrice = GetDecimal(element.Element("LastTradePriceOnly").Value);
                    quote.Name = element.Element("Name").Value;
                    quote.Open = GetDecimal(element.Element("Open").Value);
                    quote.Volume = GetDecimal(element.Element("Volume").Value);
                    quote.StockExchange = element.Element("StockExchange").Value;
                    quote.LastUpdate = DateTime.Now;
                }
                catch { }
            }
        }


        private static decimal? GetDecimal(string input)
        {
            if (input == null) return null;

            input = input.Replace("%", "");

            decimal value;

            if (Decimal.TryParse(input, out value)) return value;
            return null;
        }

        private static DateTime? GetDateTime(string input)
        {
            if (input == null) return null;

            DateTime value;

            if (DateTime.TryParse(input, out value)) return value;
            return null;
        }





        public static void SymbolInsert(Symbol symbol)
        {
            using (var db = new MyDbEntities())
            {
                try
                {
                    db.Symbols.Add(symbol);
                    db.SaveChanges();
                }
                catch { }
            }
        }

        public static void SymbolInsert(ObservableCollection<Symbol> symbols)
        {
            using (var db = new MyDbEntities())
            {
                try
                {
                    foreach (var s in symbols)
                    {
                        var symbol = new Symbol();
                        symbol = s;
                        db.Symbols.Add(symbol);
                    }
                    db.SaveChanges();
                }
                catch { }
            }
        }

        public static void SymbolInsert(string csvFile)
        {
            ObservableCollection<Symbol> symbols = CsvToSymbolCollection(csvFile);
            SymbolInsert(symbols);
        }

        public static ObservableCollection<Symbol> CsvToSymbolCollection(string csvFile)
        {
            DataTable dt = ModelHelper.CsvToDatatable(csvFile);
            ObservableCollection<Symbol> symbols = new ObservableCollection<Symbol>();
            foreach (DataRow row in dt.Rows)
            {
                symbols.Add(new Symbol
                {
                    Ticker = row["Ticker"].ToString(),
                    Region = row["Region"].ToString(),
                    Sector = row["Sector"].ToString()
                });
            }
            return symbols;
        }




        public static string IdToTicker(int symbolId)
        {
            string ticker = string.Empty;

            using (var db = new MyDbEntities())
            {
                var query = from s in db.Symbols
                            where (s.SymbolID == symbolId)
                            select s.Ticker;

                foreach (var q in query)
                    ticker = q.ToString();
            }
            return ticker;
        }

        public static void PriceInsert(Price price)
        {
            using (var db = new MyDbEntities())
            {
                try
                {
                    db.Prices.Add(price);
                    db.SaveChanges();
                }
                catch { }
            }
        }

        public static void PriceInsert(BindableCollection<Price> prices)
        {
            using (var db = new MyDbEntities())
            {
                try
                {
                    foreach (var p in prices)
                    {
                        var price = new Price();
                        price = p;
                        db.Prices.Add(price);
                    }
                    db.SaveChanges();
                }
                catch { }
            }
        }

        public static ObservableCollection<Symbol> GetTickers()
        {
            var res = new ObservableCollection<Symbol>();
            using (var db = new MyDbEntities())
            {
                try
                {
                    var query = from s in db.Symbols orderby s.Ticker select s;
                    res.AddRange(query);
                }
                catch { }
            }
            return res;
        }

        public static ObservableCollection<Price> GetYahooHistStockData(int symbolId, string ticker, DateTime startDate, DateTime endDate)
        {
            ObservableCollection<Price> res = new ObservableCollection<Price>();
            DataTable prices = GetYahooHistStockDataTable(ticker, startDate, endDate);
            foreach (DataRow r in prices.Rows)
            {
                res.Add(new Price
                {
                    SymbolID = symbolId,
                    Date = Convert.ToDateTime(r["Date"]),
                    PriceOpen = Convert.ToDouble(r["Open"]),
                    PriceHigh = Convert.ToDouble(r["High"]),
                    PriceLow = Convert.ToDouble(r["Low"]),
                    PriceClose = Convert.ToDouble(r["Close"]),
                    PriceAdj = Convert.ToDouble(r["Adj Close"]),
                    Volume = Convert.ToDouble(r["Volume"])
                });
            }
            return res;
        }
    }
}
