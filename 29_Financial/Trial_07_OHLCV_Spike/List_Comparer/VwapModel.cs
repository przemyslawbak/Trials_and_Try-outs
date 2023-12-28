namespace List_Comparer
{
    internal class VwapModel
    {
        public decimal Close { get; set; }
        public int Index { get; set; }
        public decimal TypicalPrice { get; set; }
        public decimal MultiplyTheAveragePriceWithVolume { get; set; }
        public decimal CumulativeTotalProductTypicalPriceAndVolume { get; set; }
        public decimal CumulativeTotalVol { get; set; }
        public decimal Volume { get; set; }
        public decimal VWAP { get; set; }
    }
}
