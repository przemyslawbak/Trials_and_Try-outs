namespace Sample
{
    internal class ClosedPositionOutputModel
    {
        public decimal Dropdown { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public bool IsLong { get; set; }
        public bool IsShort { get; set; }
        public string CloseTimestamp { get; set; }
        public string OpenTimestamp { get; set; }
        public List<string> OpenTimestampPut { get; set; }
        public List<decimal> DropdownPut { get; set; }
        public List<decimal> BuyPricePutList { get; set; }
        public List<decimal> SellPricePutList { get; set; }
        public string CheckSignal { get; set; }
        public string LastKnownSignal { get; set; }
        public bool IsLongFol { get; set; }
        public bool IsShortFol { get; set; }
        public decimal BuyPriceFol { get; set; }
        public decimal SellPriceFol { get; set; }
        public decimal DropdownFol { get; set; }
        public string OpenTimestampFol { get; set; }
        public string CheckSignalFol { get; set; }
        public string LastKnownSignalFol { get; set; }
        public List<decimal> DropdownsFol { get; set; }
        public List<decimal> TransactionsFol { get; set; }
        public List<double> DaysFol { get; set; }
        public string CloseTimestampFol { get; set; }
    }
}
