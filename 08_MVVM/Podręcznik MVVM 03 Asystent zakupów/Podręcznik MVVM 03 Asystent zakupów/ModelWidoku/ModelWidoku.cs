using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Podręcznik_MVVM_03_Asystent_zakupów.ModelWidoku
{
    using Model;
    using System.ComponentModel;
    using System.Windows;
    public class ModelWidoku : INotifyPropertyChanged //info o zmianach
    {
        private SumowanieKwot model = new SumowanieKwot(1000); //wywołanie modelu
        public string Suma //utworzenie własności Suma
        {
            get
            {
                return model.Suma.ToString(); //przekazanie z obiektu "model" wywołanego modelu sumy do string
            }
        }
        public event PropertyChangedEventHandler PropertyChanged; //gdy coś się zmieniło w modelu widoku
        private void OnPropertyChanged(string nazwaWłasnosci)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nazwaWłasnosci));
        }
        public bool CzyŁańcuchKwotyJestPoprawny(string s) //bool dla poprawności łańcucha kwoty
        {
            if (string.IsNullOrWhiteSpace(s)) return false; //jeśli string jest spacją, false
            decimal kwota; //jeśli string jest kwota, true
            if (!decimal.TryParse(s, out kwota)) return false; //jeśli negacja decimal parsowana jest różna od kwota, false
            else return model.CzyKwotaJestPoprawna(kwota);
        }
        private ICommand dodajKwotęCommand;
        public ICommand DodajKwotę
        {
            get
            {
                if (dodajKwotęCommand == null)
                    dodajKwotęCommand = new RelayCommand(
                    (object argument) =>
                    {
                        decimal kwota = decimal.Parse((string)argument);
                        model.Dodaj(kwota);
                        OnPropertyChanged("Suma");
                    },
                    (object argument) =>
                    {
                        return CzyŁańcuchKwotyJestPoprawny((string)argument);
                    }
                    );
                return dodajKwotęCommand;
            }
        }
    }
}
