namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputClosePosition = new InputClosePositionModel()
            {
                LastClose = input.LastClose,
                BuySignal = buySignal,
                SellSignal = sellSignal,
                BuyPrice = 4994.42500000M,
                SellPrice = computeTransactionsOutput.SellPrice,
                IsLong = computeTransactionsOutput.IsLong,
                IsShort = computeTransactionsOutput.IsShort,
                Dropdown = computeDropdownOutput.Dropdown,
                Timestamp = input.Timestamp,
                OpenTimestamp = computeTransactionsOutput.OpenTimestamp,
                CloseTimestamp = computeTransactionsOutput.CloseTimestamp,
                OpenTimestampPut = computeTransactionsOutput.OpenTimestampPut,
                BuyPricePutList = new List<decimal>() { 4994.42500000M, 4992.95500000M },
                SellPricePutList = computeTransactionsOutput.SellPricePutList,
                DropdownPut = computeDropdownOutput.DropdownPut,
                CheckSignal = computeTransactionsOutput.CheckSignal,
                LastKnownSignal = computeTransactionsOutput.LastKnownSignal,
                BuySignalFol = buySignalFol,
                SellSignalFol = sellSignalFol,
                BuyPriceFol = 4987.62500000M,
                SellPriceFol = computeTransactionsOutput.SellPriceFol,
                IsLongFol = computeTransactionsOutput.IsLongFol, // true i 92
                IsShortFol = computeTransactionsOutput.IsShortFol,
                DropdownFol = computeDropdownOutput.DropdownFol,
                OpenTimestampFol = computeTransactionsOutput.OpenTimestampFol,
                CloseTimestampFol = computeTransactionsOutput.CloseTimestampFol,
                CheckSignalFol = computeTransactionsOutput.CheckSignalFol,
                LastKnownSignalFol = computeTransactionsOutput.LastKnownSignalFol,
            };
        }
    }
}