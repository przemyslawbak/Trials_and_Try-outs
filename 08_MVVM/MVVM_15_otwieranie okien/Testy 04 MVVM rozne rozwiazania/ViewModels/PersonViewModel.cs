using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Testy_04_MVVM_rozne_rozwiazania.Models;

namespace Testy_04_MVVM_rozne_rozwiazania.ViewModels
{
    public class PersonViewModel
    {
        PersonModel osoba = new PersonModel();

        public string Wiek
        {
            get
            {
                return osoba.Age;
            }
        }

        public string Imie
        {
            get
            {
                return osoba.Name;
            }
        }

        private ICommand openWindowCommand;

        public ICommand OpenWindowCommand
        {
            get
            {
                if (openWindowCommand == null) //jeśli nie jest uruchomiony...
                    openWindowCommand = new RelayCommand( //przekazujemy do RC...
                    argument =>
                    {
                        var openWindow = new WindowDialog();
                        openWindow.ShowWindow(new PersonViewModel());
                    });
                return openWindowCommand;
            }
        }
    }





}
