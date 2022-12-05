using Financial.Commands;
using Financial.Models;
using System;
using System.Windows.Input;

namespace Financial.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Ticker = "dupa";
            InitModel();

            UpdateStock = new DelegateCommand(OnUpdateStock);
        }

        private void OnUpdateStock(object obj)
        {
            Change();
        }

        public ICommand UpdateStock { get; private set; }

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

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
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
        public double PriceClose
        {
            get => _priceClose;
            set
            {
                _priceClose = value;
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
                    Date = new DateTime(2015, 1, 14),
                    PriceOpen = 45.45,
                    PriceHigh = 45.96,
                    PriceLow = 45.31,
                    PriceClose = 45.62
                };

                toUpdate = updated;
            }
            else
            {
                Stock updated = new Stock()
                {
                    Ticker = "IBM",
                    Date = new DateTime(2015, 1, 14),
                    PriceOpen = 169.43,
                    PriceHigh = 169.54,
                    PriceLow = 168.24,
                    PriceClose = 168.61
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
                Date = new DateTime(2015, 1, 14),
                PriceOpen = 169.43,
                PriceHigh = 169.54,
                PriceLow = 168.24,
                PriceClose = 168.61,
            };

            UpdatedProperties(init);
        }

        private void UpdatedProperties(Stock init)
        {
            Ticker = init.Ticker;
            Date = init.Date;
            PriceOpen = init.PriceOpen;
            PriceHigh = init.PriceHigh;
            PriceLow = init.PriceLow;
            PriceClose = init.PriceClose;
        }
    }
}
