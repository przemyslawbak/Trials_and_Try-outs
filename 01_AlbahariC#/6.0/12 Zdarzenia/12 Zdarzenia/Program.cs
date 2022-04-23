using System;

namespace _12_Zdarzenia
{
    public class PriceChangedEventArgs : EventArgs //"Jest to klasa bazowa do przekazywania informacji dla zdarzenia."
    {
        public readonly decimal LastPrice; //argument 1
        public readonly decimal NewPrice; //argument 2
        public PriceChangedEventArgs(decimal lastPrice, decimal newPrice) //konstruktor
        {
            LastPrice = lastPrice; NewPrice = newPrice;
        }
    }
    public class Stock
    {
        string symbol; //nazwa
        decimal price; //cena
        public Stock(string symbol) //konstruktor
        {
            this.symbol = symbol;
        }

        public event EventHandler<PriceChangedEventArgs> PriceChanged; // "Po zdefiniowaniu podklasy klasy EventArgs jest wybór lub zdefiniowanie delegatu dla zdarzenia"

        //public event EventHandler PriceChanged;

        protected virtual void OnPriceChanged(PriceChangedEventArgs e) // "I w końcu wzorzec zakłada napisanie chronionej metody wirtualnej uruchamiającej zdarzenie."
        {
            PriceChanged?.Invoke(this, e);
        }

        /*
         * protected virtual void OnPriceChanged (EventArgs e)
        {
        PriceChanged?.Invoke (this, e);
        }
        */
        public decimal Price
        {
            get { return price; }
            set
            {
                if (price == value) return;
                decimal oldPrice = price;
                price = value;
                OnPriceChanged(new PriceChangedEventArgs(oldPrice, price));
            }
        }
    }
    class Test
    {
        static void Main()
        {
            Stock stock = new Stock("THPW");
            stock.Price = 27.10M;
            // rejestracja w zdarzeniu PriceChanged
            stock.PriceChanged += stock_PriceChanged;
            stock.Price = 31.59M;
        }
        static void stock_PriceChanged(object sender, PriceChangedEventArgs e)
        {
            if ((e.NewPrice - e.LastPrice) / e.LastPrice > 0.1M)
                Console.WriteLine("Alert, 10% stock price increase!");
        }
    }
}
