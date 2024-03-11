namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var simpleObject = new SimpleModel()
            {
                NumberInt = 1,
                Text = "Sample Text",
            };

            var yyy = ProcessSampleObject(simpleObject);


            var inputClosePosition = new InputClosePositionModel()
            {
                LastClose = 4987.62500000M,
                BuySignal = false,
                SellSignal = false,
                BuyPrice = 4994.42500000M,
                SellPrice = 0,
                IsLong = true,
                IsShort = false,
                Dropdown = -6.80000000M,
                Timestamp = "2024-02-07 15:39",
                OpenTimestamp = "2024-02-07 15:30",
                CloseTimestamp = "2024-02-07 15:30",
                OpenTimestampPut = new List<string>() { "2024-02-07 15:30", "2024-02-07 15:34" },
                BuyPricePutList = new List<decimal>() { 4994.42500000M, 4992.95500000M },
                SellPricePutList = new List<decimal>(),
                DropdownPut = new List<decimal>() { -6.80000000M },
                CheckSignal = "NoSignalNoSignal",
                LastKnownSignal = "NoSignal",
                BuySignalFol = false,
                SellSignalFol = false,
                BuyPriceFol = 4987.62500000M,
                SellPriceFol = 4990.95500000M,
                IsLongFol = true, // true i 92
                IsShortFol = false,
                DropdownFol = -3.47000000M,
                OpenTimestampFol = "2024-02-07 15:39",
                CloseTimestampFol = "2024-02-07 15:30",
                CheckSignalFol = "NoSignalbuySignal",
                LastKnownSignalFol = "NoSignal",
            };

            var closePositionOutput = ClosePositionIfNeed(inputClosePosition);
        }

        private static SimpleModel ProcessSampleObject(SimpleModel simpleObject)
        {
            simpleObject.Text = "DUPA";
            simpleObject.NumberInt = 666;

            return new SimpleModel()
            {
                Text = simpleObject.Text,
                NumberInt = simpleObject.NumberInt,
            };
        }

        private static ClosedPositionOutputModel ClosePositionIfNeed(InputClosePositionModel inputClosePosition)
        {
            var shallowCopy = inputClosePosition;
            //var shallowCopy = (InputClosePositionModel)inputClosePosition.Clone(); 
            if (shallowCopy.BuyPrice != 0 && shallowCopy.SellPrice != 0) //_results _putResults
            {
                shallowCopy.BuyPrice = 0;
                shallowCopy.SellPrice = 0;
                shallowCopy.IsLong = false;
                shallowCopy.IsShort = false;
                shallowCopy.Dropdown = 0;
                shallowCopy.OpenTimestamp = shallowCopy.Timestamp;
                shallowCopy.OpenTimestampPut = new List<string>();

                shallowCopy.DropdownPut = new List<decimal>();
                shallowCopy.BuyPricePutList = new List<decimal>();
                shallowCopy.SellPricePutList = new List<decimal>();

                if (shallowCopy.BuySignal == true)
                {
                    shallowCopy.BuyPrice = shallowCopy.LastClose;
                    shallowCopy.IsLong = true;

                    shallowCopy.OpenTimestampPut.Add(shallowCopy.Timestamp);
                    shallowCopy.BuyPricePutList.Add(shallowCopy.BuyPrice);

                    if (shallowCopy.BuySignalFol == shallowCopy.BuySignal /*|| inputClosePosition.IsLongFol == true*/)
                    {
                        shallowCopy.OpenTimestampFol = shallowCopy.Timestamp;
                        shallowCopy.BuyPriceFol = shallowCopy.LastClose;
                        shallowCopy.IsLongFol = true;
                    }
                }

                if (shallowCopy.SellSignal == true)
                {
                    shallowCopy.SellPrice = shallowCopy.LastClose;
                    shallowCopy.IsShort = true;

                    shallowCopy.OpenTimestampPut.Add(shallowCopy.Timestamp);
                    shallowCopy.SellPricePutList.Add(shallowCopy.SellPrice);

                    if (shallowCopy.SellSignalFol == shallowCopy.SellSignal /*|| inputClosePosition.IsShortFol == true*/)
                    {
                        shallowCopy.OpenTimestampFol = shallowCopy.Timestamp;
                        shallowCopy.SellPriceFol = shallowCopy.LastClose;
                        shallowCopy.IsShortFol = true;
                    }
                }

            }

            // followed+following 
            if (shallowCopy.BuyPriceFol != 0 && shallowCopy.SellPriceFol != 0)
            {
                shallowCopy.BuyPriceFol = 0;
                shallowCopy.SellPriceFol = 0;
                shallowCopy.IsLongFol = false;
                shallowCopy.IsShortFol = false;
                shallowCopy.DropdownFol = 0;
                if (shallowCopy.BuySignalFol == true && shallowCopy.BuySignal == true)// dla is long = =true ||
                {
                    shallowCopy.BuyPriceFol = shallowCopy.LastClose;
                    shallowCopy.IsLongFol = true;
                    shallowCopy.OpenTimestampFol = shallowCopy.Timestamp;
                }
                if (shallowCopy.SellSignalFol == true && shallowCopy.SellSignal == true)// adekwatnie do wyzej
                {
                    shallowCopy.SellPriceFol = shallowCopy.LastClose;
                    shallowCopy.IsShortFol = true;
                    shallowCopy.OpenTimestampFol = shallowCopy.Timestamp;
                }
            }
            var toReturn = new ClosedPositionOutputModel()
            {
                BuyPrice = shallowCopy.BuyPrice,
                SellPrice = shallowCopy.SellPrice,
                CloseTimestamp = shallowCopy.CloseTimestamp,
                Dropdown = shallowCopy.Dropdown,
                IsLong = shallowCopy.IsLong,
                IsShort = shallowCopy.IsShort,
                OpenTimestamp = shallowCopy.OpenTimestamp,
                BuyPricePutList = shallowCopy.BuyPricePutList,
                SellPricePutList = shallowCopy.SellPricePutList,
                OpenTimestampPut = shallowCopy.OpenTimestampPut,
                DropdownPut = shallowCopy.DropdownPut,
                CheckSignal = shallowCopy.CheckSignal,
                LastKnownSignal = shallowCopy.LastKnownSignal,
                IsLongFol = shallowCopy.IsLongFol,
                IsShortFol = shallowCopy.IsShortFol,
                BuyPriceFol = shallowCopy.BuyPriceFol,
                SellPriceFol = shallowCopy.SellPriceFol,
                CheckSignalFol = shallowCopy.CheckSignalFol,
                LastKnownSignalFol = shallowCopy.LastKnownSignalFol,
                OpenTimestampFol = shallowCopy.OpenTimestampFol,
                CloseTimestampFol = shallowCopy.CloseTimestampFol,
                DropdownFol = shallowCopy.DropdownFol,
            };
            return toReturn;
        }
    }
}