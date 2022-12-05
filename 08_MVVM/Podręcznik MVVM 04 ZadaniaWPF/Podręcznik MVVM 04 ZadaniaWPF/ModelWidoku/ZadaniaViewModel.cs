using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows;

namespace Podręcznik_MVVM_04_ZadaniaWPF.ModelWidoku
{
    public class ZadaniaViewModel
    {
        private const string ścieżkaPlikuXml = "zadania.xml"; //utworzenie ścieżki pliku
        //przechowywanie dwóch kolekcji
        private Model.Zadania model; //wywołanie klasy zadania

        //dzięki observable collection ListBox z xaml będzie aktualna
        public ObservableCollection<ZadanieViewModel> ListaZadań { get; } = new ObservableCollection<ZadanieViewModel>();
        private void KopiujZadania()
        {
            ListaZadań.CollectionChanged -= SynchronizacjaModelu;
            ListaZadań.Clear();
            foreach (Model.Zadanie zadanie in model)
                ListaZadań.Add(new ZadanieViewModel(zadanie));
            ListaZadań.CollectionChanged += SynchronizacjaModelu;
        }
        public ZadaniaViewModel()
        {
            if (System.IO.File.Exists(ścieżkaPlikuXml))
                model = Model.PlikXML.Czytaj(ścieżkaPlikuXml);
            else model = new Model.Zadania();



            KopiujZadania();
        }
        private void SynchronizacjaModelu(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ZadanieViewModel noweZadanie = (ZadanieViewModel)e.NewItems[0];
                    if (noweZadanie != null)
                        model.DodajZadanie(noweZadanie.GetModel());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ZadanieViewModel usuwaneZadanie = (ZadanieViewModel)e.OldItems[0];
                    if (usuwaneZadanie != null)
                        model.UsuńZadanie(usuwaneZadanie.GetModel());
                    break;
            }
        }

        //komendy

        //zapisywanie przy zamykaniu
        private ICommand zapiszCommand;
        public ICommand Zapisz //wywołanie przez trigger w XAML
        {
            get
            {
                if (zapiszCommand == null)
                    zapiszCommand = new RelayCommand(
                    argument =>
                    {
                        Model.PlikXML.Zapisz(ścieżkaPlikuXml, model);
                    });
                return zapiszCommand;
            }
        }
        private ICommand usuńZadanie;
        public ICommand UsuńZadanie
        {
            get
            {
                if (usuńZadanie == null)
                    usuńZadanie = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        int indeksZadania = (int)o;
                        ZadanieViewModel zadanie = ListaZadań[indeksZadania];

                        ListaZadań.Remove(zadanie);
                    },
                    o => //parametr "o" wyrażenia lambda
                    {
                        if (o == null) return false;
                        int indeksZadania = (int)o;
                        return indeksZadania >= 0;
                    });
                return usuńZadanie;
            }
        }
        private ICommand dodajZadanie;
        public ICommand DodajZadanie
        {
            get
            {
                if (dodajZadanie == null)
                    dodajZadanie = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        ZadanieViewModel zadanie = o as ZadanieViewModel;
                        if (zadanie != null) ListaZadań.Add(zadanie);
                    },
                    o => //parametr "o" wyrażenia lambda
                    {
                        return (o as ZadanieViewModel) != null;
                    });
                return dodajZadanie;
            }
        }

        private ICommand sortujZadania;
        public ICommand SortujZadania
        {
            get
            {
                if (sortujZadania == null)
                    sortujZadania = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        bool porównywaniePriorytetówCzyPlanowanychTerminówRealizacji =
                        bool.Parse((string)o);
                        model.SortujZadania(
                        porównywaniePriorytetówCzyPlanowanychTerminówRealizacji);
                        KopiujZadania();
                    });
                return sortujZadania;
            }
        }

        private ICommand eksportujZadaniaDoPlikuTekstowego;

        //do tego odwołuje się przycisk "Eksportuj..." z widoku
        public ICommand EksportujZadaniaDoPlikuTekstowego
        {
            get
            {
                if (eksportujZadaniaDoPlikuTekstowego == null) // jeśli nie jest aktywne
                    eksportujZadaniaDoPlikuTekstowego = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        string ścieżkaPliku = (string)o; //ścieżka pliku to string z parametru "o"
                        Model.PlikTXT.Zapisz(ścieżkaPliku, model);
                    });
                return eksportujZadaniaDoPlikuTekstowego;
            }
        }
    }
}