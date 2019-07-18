using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Testy_05_szukanie_console
{
    public class AdresViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

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

        private AdresModel model;

        public string AdresMailowy
        {
            get
            {
                return model.AdresMailowy;
            }
        }

        public bool AktywnyEmail
        {
            get
            {
                return model.AktywnyEmail;
            }
            set
            {
                model.AktywnyEmail = value;
                OnPropertyChanged("AktywnyEmail");
            }
        }

        public bool ZaznaczenieEmail
        {
            get
            {
                return model.ZaznaczenieEmail;
            }
            set
            {
                model.ZaznaczenieEmail = value;
                OnPropertyChanged("ZaznaczenieEmail");
            }
        }

        public AdresViewModel(AdresModel adres)
        {
            model = adres;
        }




        public AdresViewModel(string adresEmail, bool aktywnyEmail, bool zaznaczenieEmail)
        {
            model = new AdresModel(adresEmail, aktywnyEmail, zaznaczenieEmail);
        }

        public AdresModel GetModel()
        {
            return model;
        }

    }

    public class AdresyEmailViewModel : INotifyPropertyChanged


    {
        public event PropertyChangedEventHandler PropertyChanged;
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



        private const string saveAdresPathXml = "adresy.xml";
        private AdresyEmail model;

        public static ObservableCollection<AdresViewModel> ListaAdresow { get; } = new ObservableCollection<AdresViewModel>();

        private void KopiujAdresy()
        {
            ListaAdresow.CollectionChanged -= ModelSynchro;
            ListaAdresow.Clear();
            foreach (AdresModel adres in model)
                ListaAdresow.Add(new AdresViewModel(adres));
            ListaAdresow.CollectionChanged += ModelSynchro;
        }
        private void ModelSynchro(object sender, NotifyCollectionChangedEventArgs e) //wywołuje akcję, wchodzimy przez CopyPersons()
        {
            switch (e.Action)
            {
                //nazwy metod jak w modelu: DelPerson i NewPerson
                //NotifyCollectionChangedAction opisuje akcje które spowodowały CollectionChanged event
                case NotifyCollectionChangedAction.Add: //Describes the action that caused a System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged
                    AdresViewModel newTask = (AdresViewModel)e.NewItems[0]; //NewItems otrzymuje listę nowych rzeczy powodujących zmianę.
                    if (newTask != null) //jeśli wciąż są nowe itmey...
                        model.DodajAdres(newTask.GetModel()); //w kolekcji jest tworzone nowe wywołanie modelu
                    break;
                case NotifyCollectionChangedAction.Remove: //j.w. tylko dla usuwania
                    AdresViewModel removeTask = (AdresViewModel)e.OldItems[0];
                    if (removeTask != null)
                        model.UsunAdres(removeTask.GetModel());
                    break;
            }
        }
        public AdresyEmailViewModel()
        {




        }






    }
}
