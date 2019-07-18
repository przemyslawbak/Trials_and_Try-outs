using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_01_MVVM_IController.ViewModel
{
    public class PersonsList : INotifyPropertyChanged //klasa modelu kolekcji
    {
        private Model.Person model; //instancja modelu osoby


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged; //event dla INotifyPropertyChanged

        private void OnPropertyChanged(params string[] propertyNames) //było wykorzystywane do odznaczania jako zrealizowane
        {
            if (PropertyChanged != null) //jeśli zmiana własności...
            {   //foreach korzyst z własności modelu IEnumerator IEnumerable.GetEnumerator()
                foreach (string nazwaWłasności in propertyNames) //dla każdej nazwy własności z nazwWłasności
                    PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));//odznacza zmienione własności (chyba)
            }
        }

        #endregion
        //własności
        #region Properties 
        public string ID
        {
            get
            {
                return model.ID; //własność
            }
        }

        public string FirstName
        {
            get
            {
                return model.FirstName;//własność
            }
        }

        public string SecondName
        {
            get
            {
                return model.SecondName;//własność
            }
        }

        public Model.HeightList Height
        {
            get
            {
                return model.Height;//własność
            }
        }

        #endregion

        #region Konstruktor
        public PersonsList(Model.Person person) //konstuktor
        {
            model = person; //przy wczytywaniu wywoływany jest konstruktor dla dodania rekordu
        }

        public PersonsList(string id, string firstName, string secondName, Model.HeightList heightList)
        {   //konstruktor, odwołuje się konwerter dla instancji modelu kolekcji
            model = new Model.Person(id, firstName, secondName, heightList);
        }



        public Model.Person GetModel() //zwraca model jako kolekcję
        {
            return model;
        }

        #endregion

        #region Commands



        #endregion

    }
}
