using Financial.Commands;
using Financial.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Windows.Input;

namespace Financial.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            //InitModel();

            RetreiveStock = new DelegateCommand(OnRetreiveStock);
        }

        private async void OnRetreiveStock(object obj)
        {
            //Change();
            await GetFromApiAsync();
        }

        //powinno być w serwisie
        private async System.Threading.Tasks.Task GetFromApiAsync()
        {
            Ticker = "AAPL";
            string page = "https://finnhub.io/api/v1/quote?symbol=" + Ticker + "&token=xxx";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(page))
                    using (HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();

                        JObject joResponse = JObject.Parse(result);

                        if (!string.IsNullOrEmpty(joResponse.SelectToken("c").ToString())) PriceCurrent = double.Parse(joResponse.SelectToken("c").ToString());
                        if (!string.IsNullOrEmpty(joResponse.SelectToken("o").ToString())) PriceOpen = double.Parse(joResponse.SelectToken("o").ToString());
                        if (!string.IsNullOrEmpty(joResponse.SelectToken("h").ToString())) PriceHigh = double.Parse(joResponse.SelectToken("h").ToString());
                        if (!string.IsNullOrEmpty(joResponse.SelectToken("l").ToString())) PriceLow = double.Parse(joResponse.SelectToken("l").ToString());
                        if (!string.IsNullOrEmpty(joResponse.SelectToken("pc").ToString())) PreviousPriceClose = double.Parse(joResponse.SelectToken("pc").ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public ICommand RetreiveStock { get; private set; }

        private string _ticker;
        public string Ticker
        {
            get => _ticker;
            set
            {
                _ticker = value;
                OnPropertyChanged();
            }
        }

        private double _priceOpen;
        public double PriceOpen
        {
            get => _priceOpen;
            set
            {
                _priceOpen = value;
                OnPropertyChanged();
            }
        }

        private double _priceHigh;
        public double PriceHigh
        {
            get => _priceHigh;
            set
            {
                _priceHigh = value;
                OnPropertyChanged();
            }
        }

        private double _priceLow;
        public double PriceLow
        {
            get => _priceLow;
            set
            {
                _priceLow = value;
                OnPropertyChanged();
            }
        }

        private double _priceClose;
        public double PreviousPriceClose
        {
            get => _priceClose;
            set
            {
                _priceClose = value;
                OnPropertyChanged();
            }
        }

        private double _priceCurrent;
        public double PriceCurrent
        {
            get => _priceCurrent;
            set
            {
                _priceCurrent = value;
                OnPropertyChanged();
            }
        }

        public void Change()
        {
            Stock toUpdate = new Stock();

            if (Ticker == "IBM")
            {
                Stock updated = new Stock()
                {
                    Ticker = "MSFT",
                    PriceOpen = 45.45,
                    PriceHigh = 45.96,
                    PriceLow = 45.31,
                    PreviousPriceClose = 45.62
                };

                toUpdate = updated;
            }
            else
            {
                Stock updated = new Stock()
                {
                    Ticker = "IBM",
                    PriceOpen = 169.43,
                    PriceHigh = 169.54,
                    PriceLow = 168.24,
                    PreviousPriceClose = 168.61
                };

                toUpdate = updated;
            }

            UpdatedProperties(toUpdate);
        }
        private void InitModel()
        {
            Stock init = new Stock
            {
                Ticker = "IBM",
                PriceOpen = 169.43,
                PriceHigh = 169.54,
                PriceLow = 168.24,
                PreviousPriceClose = 168.61,
            };

            UpdatedProperties(init);
        }

        private void UpdatedProperties(Stock init)
        {
            Ticker = init.Ticker;
            PriceOpen = init.PriceOpen;
            PriceHigh = init.PriceHigh;
            PriceLow = init.PriceLow;
            PreviousPriceClose = init.PreviousPriceClose;
        }
    }
}
