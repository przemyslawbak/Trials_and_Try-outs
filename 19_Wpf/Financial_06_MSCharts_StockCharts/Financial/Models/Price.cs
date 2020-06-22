using System;

namespace Financial.Models
{
    public class Price
    {
        public int SymbolID { get; set; }
        public DateTime Date { get; set; }
        public double PriceOpen { get; set; }
        public double PriceHigh { get; set; }
        public double PriceLow { get; set; }
        public double PriceClose { get; set; }
        public double PriceAdj { get; set; }
        public double Volume { get; set; }
    }
}