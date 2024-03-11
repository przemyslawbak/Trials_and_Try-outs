namespace Sample
{
    public class InputClosePositionModel
    {
        public decimal LastClose { get; set; }
        public bool BuySignal { get; set; }
        public bool SellSignal { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public bool IsLong { get; set; }
        public bool IsShort { get; set; }
        public decimal Dropdown { get; set; }
        public string Timestamp { get; set; }
        public string OpenTimestamp { get; set; }
        public string CloseTimestamp { get; set; }
        public List<string> OpenTimestampPut { get; set; }
        public List<decimal> BuyPricePutList { get; set; }
        public List<decimal> SellPricePutList { get; set; }
        public List<decimal> DropdownPut { get; set; }
        public string CheckSignal { get; set; }
        public string LastKnownSignal { get; set; }
        public bool IsLongFol { get; set; }
        public bool IsShortFol { get; set; }
        public bool BuySignalFol { get; set; }
        public bool SellSignalFol { get; set; }
        public decimal BuyPriceFol { get; set; }
        public decimal SellPriceFol { get; set; }
        public decimal DropdownFol { get; set; }
        public string OpenTimestampFol { get; set; }
        public string CloseTimestampFol { get; set; }
        public string CheckSignalFol { get; set; }
        public string LastKnownSignalFol { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
