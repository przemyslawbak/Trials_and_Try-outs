using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;



namespace Testy_01_MVVM_IController.ViewModel
{
    public class Persons
    {
        private const string savePathXml = "persons.xml"; //zmienna ścieżki

        private Model.Persons model; //instancja modelu

        //     Represents a dynamic data collection that provides notifications when items get
        //     added, removed, or when the whole list is refreshed.
        //     : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
        public static ObservableCollection<PersonsList> PersonsList { get; } = new ObservableCollection<PersonsList>(); //zasysa MV

        private void CopyPersons() //wczytywanie z pliku
        {               //CollectionChanged to jest event który się pojawia, gdy item jest dodany, usunięty itp
            PersonsList.CollectionChanged -= ModelSynchro; //na początek wył. akcji synchro(?), pewnie na wypadek gdyby było użyte wcześniej
            PersonsList.Clear(); //wyczyszczenie kolekcji
            foreach (Model.Person person in model) //dla każdego obuiektu modelu z kolekcji modelu IEnumerator<Person>...
                PersonsList.Add(new PersonsList(person)); //dodaje model osoby w kolekcji
            PersonsList.CollectionChanged += ModelSynchro; //wł. akcji synchro
        }

        public Persons() //konstruktor, wołany przez Window.DataContext z xaml
        {
            if (System.IO.File.Exists(savePathXml)) //jeśli plik istnieje to...
                model = Model.SavingXML.Load(savePathXml); //wołanie modelem(kolekcją) metody Load w SavingXML
            else model = new Model.Persons(); //w przeciwnym wypadku utwórz nową kolekcję

            CopyPersons(); //wołanie synchronizacji modelu i NotifyCollectionChangedAction.Add
        }

        private void ModelSynchro(object sender, NotifyCollectionChangedEventArgs e) //wywołuje akcję, wchodzimy przez CopyPersons()
        {
            switch (e.Action)
            {
                //nazwy metod jak w modelu: DelPerson i NewPerson
                //NotifyCollectionChangedAction opisuje akcje które spowodowały CollectionChanged event
                case NotifyCollectionChangedAction.Add: //Describes the action that caused a System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged
                    PersonsList newTask = (PersonsList)e.NewItems[0]; //NewItems otrzymuje listę nowych rzeczy powodujących zmianę.
                    if (newTask != null) //jeśli wciąż są nowe itmey...
                        model.NewPerson(newTask.GetModel()); //w kolekcji jest tworzone nowe wywołanie modelu
                    break;
                case NotifyCollectionChangedAction.Remove: //j.w. tylko dla usuwania
                    PersonsList removeTask = (PersonsList)e.OldItems[0];
                    if (removeTask != null)
                        model.DelPerson(removeTask.GetModel());
                    break;
            }
        }

        //komendy

        private ICommand delPerson;

        public ICommand DelPerson
        {
            get
            {
                if (delPerson == null) //jak delPerson jeszcze nie było
                    delPerson = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        int indeksZadania = (int)o; //indeksZadania = indeks obiektu
                        PersonsList zadanie = PersonsList[indeksZadania]; //przypisanie zmiennej zadanie obiekt z kolekcji

                        PersonsList.Remove(zadanie); //z kolekcji usuwamy przypisany zmiennej obiekt
                    },
                    o => //parametr "o" wyrażenia lambda
                    {
                        if (o == null) return false; //jeśli nie ma obiektu...
                        int indeksZadania = (int)o; //indeksZadania = indeks obiektu (ale po co?)
                        return indeksZadania >= 0; //zwracamy indeksZadania >= 0; (nie może być chyba <0)
                    });
                return delPerson; //wykonujemy RelayCommand
            }
        }

        private ICommand newPerson;
        public ICommand NewPerson
        {
            get
            {
                if (newPerson == null)
                    newPerson = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        PersonsList zadanie = o as PersonsList;
                        if (zadanie != null) PersonsList.Add(zadanie);
                    },
                    o => //parametr "o" wyrażenia lambda
                    {
                        return (o as PersonsList) != null;
                    });
                return newPerson;
            }
        }

        private ICommand savePersons;
        public ICommand SavingPers //wywołanie przez trigger w XAML
        {
            get
            {
                if (savePersons == null) //jeśli nie jest uruchomiony...
                    savePersons = new RelayCommand( //przekazujemy do RC...
                    argument =>
                    {
                        Model.SavingXML.Save(savePathXml, model); //...metodę Save z klasy SavingXML
                    });
                return savePersons;
            }
        }

        private ICommand nextPerson;

        public ICommand PrevPerson //zamiana z racji kolejności przeglądania
        {
            get
            {
                if (nextPerson == null)
                    nextPerson = new RelayCommand(
                        argument =>
                        {
                            ICollectionView widok = CollectionViewSource.GetDefaultView(PersonsList); //przekazuje metodę MoveCurrentToNext
                            widok.MoveCurrentToNext();

                            if (widok.IsCurrentAfterLast) widok.MoveCurrentToFirst(); //przenosimy do pierwszego, jeśli obecny jest ostatni
                        }
                        );
                return nextPerson;
            }
        }

        private ICommand prevPerson;

        public ICommand NextPerson //zamiana z racji kolejności przeglądania
        {
            get
            {
                if (prevPerson == null)
                    prevPerson = new RelayCommand(
                        argument =>
                        {
                            ICollectionView widok = CollectionViewSource.GetDefaultView(PersonsList);
                            widok.MoveCurrentToPrevious();

                            if (widok.IsCurrentBeforeFirst) widok.MoveCurrentToLast();
                        }
                        );
                return prevPerson;
            }
        }







    }
}
