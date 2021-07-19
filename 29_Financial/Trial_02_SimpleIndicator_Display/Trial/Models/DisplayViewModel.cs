using Skender.Stock.Indicators;
using System.Collections.Generic;

namespace Trial.Models
{
    public class DisplayViewModel
    {
        public List<SmaResult> SmaResults { get; set; }
        public List<decimal> StockPrices { get; set; }
    }
}
