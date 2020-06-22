using System;

namespace Financial.Models
{
    public class StockQuote : PropertyChangedBase
    {
        public StockQuote(string ticker)
        {
            symbol = ticker;
        }

        private string symbol;
        private decimal? bid;
        private decimal? ask;
        private decimal? percentChange;
        private decimal? change;
        private DateTime? lastTradeTime;
        private decimal? dailyLow;
        private decimal? dailyHigh;
        private decimal? yearlyLow;
        private decimal? yearlyHigh;
        private decimal? lastTradePrice;
        private string name;
        private decimal? open;
        private decimal? volume;
        private string stockExchange;
        private DateTime lastUpdate;

        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                NotifyOfPropertyChange(() => Symbol);
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
                NotifyOfPropertyChange(() => LastUpdate);
            }
        }

        public DateTime? LastTradeTime
        {
            get { return lastTradeTime; }
            set
            {
                lastTradeTime = value;
                NotifyOfPropertyChange(() => LastTradeTime);
            }
        }


        public decimal? Ask
        {
            get { return ask; }
            set
            {
                ask = value;
                NotifyOfPropertyChange(() => Ask);
            }
        }


        public decimal? Bid
        {
            get { return bid; }
            set
            {
                bid = value;
                NotifyOfPropertyChange(() => Bid);
            }
        }

        public decimal? LastTradePrice
        {
            get { return lastTradePrice; }
            set
            {
                lastTradePrice = value;
                NotifyOfPropertyChange(() => LastTradePrice);
            }
        }


        public decimal? Open
        {
            get { return open; }
            set
            {
                open = value;
                NotifyOfPropertyChange(() => Open);
            }
        }


        public string StockExchange
        {
            get { return stockExchange; }
            set
            {
                stockExchange = value;
                NotifyOfPropertyChange(() => StockExchange);
            }
        }



        public decimal? Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                NotifyOfPropertyChange(() => Volume);
            }
        }





        public decimal? DailyHigh
        {
            get { return dailyHigh; }
            set
            {
                dailyHigh = value;
                NotifyOfPropertyChange(() => DailyHigh);
            }
        }


        public decimal? DailyLow
        {
            get { return dailyLow; }
            set
            {
                dailyLow = value;
                NotifyOfPropertyChange(() => DailyLow);
            }
        }


        public decimal? Change
        {
            get { return change; }
            set
            {
                change = value;
                NotifyOfPropertyChange(() => Change);
            }
        }


        public decimal? PercentChange
        {
            get { return percentChange; }
            set
            {
                percentChange = value;
                NotifyOfPropertyChange(() => PercentChange);
            }
        }


        public decimal? YearlyHigh
        {
            get { return yearlyHigh; }
            set
            {
                yearlyHigh = value;
                NotifyOfPropertyChange(() => YearlyHigh);
            }
        }


        public decimal? YearlyLow
        {
            get { return yearlyLow; }
            set
            {
                yearlyLow = value;
                NotifyOfPropertyChange(() => YearlyLow);
            }
        }



    }
}