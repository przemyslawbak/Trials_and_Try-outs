using System;

namespace Financial.Models
{
    public class Stock
    {
        public string Ticker { get; set; }

        public DateTime Date { get; set; }

        public double PriceOpen { get; set; }

        public double PriceHigh { get; set; }

        public double PriceLow { get; set; }

        public double PriceClose { get; set; }

        public double Volume { get; set; }
    }
}
