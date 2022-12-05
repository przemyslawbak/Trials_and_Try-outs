using System;

namespace Financial.Models
{
    public class Stock
    {
        public string Ticker { get; set; }

        public double PriceOpen { get; set; }

        public double PriceHigh { get; set; }

        public double PriceLow { get; set; }
        public double PriceCurrent { get; set; }

        public double PreviousPriceClose { get; set; }

        public double Volume { get; set; }
    }
}
