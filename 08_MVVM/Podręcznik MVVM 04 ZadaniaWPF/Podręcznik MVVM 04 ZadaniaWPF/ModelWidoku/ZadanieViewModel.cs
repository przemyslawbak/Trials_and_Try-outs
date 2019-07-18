using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Podręcznik_MVVM_04_ZadaniaWPF.ModelWidoku
{
    //INotifyPropertyChanged monitoruje, czy własności uległy zmianie
    public class ZadanieViewModel : INotifyPropertyChanged //klasa i deklaracja implementacji interfejsu
    {
        private Model.Zadanie model; //utworzenie zmiennej model

        public event PropertyChangedEventHandler PropertyChanged; //zdarzenie z którego korzysta XAML, 
                                                                  //implementacja interfejsu INotifyPropertyChanged,
                                                                  //wskazuje w ten sposób własności, których wartość uległa zmianie.
        private void OnPropertyChanged(params string[] nazwyWłasności) //metoda pomocnicza
                                                                       //aktualizowanie kontrolek powiązanych 
                                                                       //z własnościami tej klasy.
                                                                       //string[] nazwyWłasności-tablica nazw własności
                                                                       //dzięki "params" nie tworzymy tablicy
        {
            if (PropertyChanged != null) //jeśli zmiana własności...
            {
                foreach (string nazwaWłasności in nazwyWłasności) //dla każdej nazwy własności z nazwyWłasności
                    PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));
            }
        }



        public string Opis
        {
            get
            {
                return model.Opis; //zwraca opis z modelu
            }
        }
        public Model.PriorytetZadania Priorytet
        {
            get
            {
                return model.Priorytet; //zwraca priorytet z modelu
            }
        }
        public DateTime DataUtworzenia
        {
            get
            {
                return model.DataUtworzenia; //zwraca datę z modelu
            }
        }
        public DateTime PlanowanyTerminRealizacji
        {
            get
            {
                return model.PlanowanyTerminRealizacji; //zwraca termin z modelu
            }
        }
        public bool CzyZrealizowane
        {
            get
            {
                return model.CzyZrealizowane; //zwraca realizacja z modelu
            }
        }
        public bool CzyZadaniePozostajeNiezrealizowanePoPlanowanymTerminie
        {
            get
            {
                return !CzyZrealizowane && (DateTime.Now > PlanowanyTerminRealizacji);  ////zwraca `czy po terminie` z modelu
            }
        }
        public ZadanieViewModel(Model.Zadanie zadanie)  //konstruktor, podajemy wartości poszczególnych własności zadania
        {
            this.model = zadanie;
        }
        public ZadanieViewModel(string opis, DateTime dataUtworzenia, //konstruktor, wykorzystany przez Konwerter i zapis
        DateTime planowanyTerminRealizacji, Model.PriorytetZadania priorytetZadania,
        bool czyZrealizowane)
        {
            // tworzy instancję (obiekt) modelu
            model = new Model.Zadanie(opis, dataUtworzenia, planowanyTerminRealizacji, priorytetZadania, czyZrealizowane);
        }

        //służy do „wyłuskania” modelu za pomocą metody GetModel
        public Model.Zadanie GetModel() //metoda
        {
            return model; //zwraca instancję modelu
        }



        ICommand oznaczJakoZrealizowane;
        public ICommand OznaczJakoZrealizowane //oznaczało zadanie jako zrealizowane, bindowanie z XAML, dla widoku przycisku
        {
            get
            {
                if (oznaczJakoZrealizowane == null) //jeśli nie jest oznaczone jako zrealizowane...
                    oznaczJakoZrealizowane = new RelayCommand( //...przekazanie do RC...
                    o =>
                    {
                        model.CzyZrealizowane = true;
                        OnPropertyChanged("CzyZrealizowane", "CzyZadaniePozostajeNiezrealizowanePoPlanowanymTerminie"); //właściwości się zmieniają na to
                    },
                    o =>
                    {
                        return !model.CzyZrealizowane;
                    });
                return oznaczJakoZrealizowane; //w przeciwnym wypadku zwróć oznaczone jako zrealizowane
            }
        }
        ICommand oznaczJakoNiezrealizowane = null;
        public ICommand OznaczJakoNiezrealizowane //oznaczało zadanie jako niezrealizowane, bindowanie z XAML, dla widoku przycisku
        {
            get
            {
                if (oznaczJakoNiezrealizowane == null) //jeśli nie jest oznaczone jako niezrealizowane...
                    oznaczJakoNiezrealizowane = new RelayCommand( //wykorzystuje klasę RC
                    o =>
                    {
                        model.CzyZrealizowane = false;
                        OnPropertyChanged("CzyZrealizowane", "CzyZadaniePozostajeNiezrealizowanePoPlanowanymTerminie");
                    },
                    o =>
                    {
                        return model.CzyZrealizowane;
                    });
                return oznaczJakoNiezrealizowane; //w przeciwnym wypadku zwróć oznaczone jako niezrealizowane
            }
        }
    }
}
