using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_01_MVVM_IController.Model
{
    public class Persons : IEnumerable<Person> //Model i kolekcja, odwołuje się MV przy instancji oraz Saving
    {
        private List<Person> personsList = new List<Person>(); //instancja kolekscji


        public void NewPerson(Person person) //odwołuje się VM dla GetModel przy dodawaniu osoby
        {
            personsList.Add(person); //dodanie nowej osoby do kolekcji
        }

        public IEnumerator<Person> GetEnumerator() //interfejs - odwołuje się VM dla foreach przy wczytywaniu z pliku, korzysta datacontext vm z xaml
        {
            return personsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() //interfejs - odwołuje się model kolekcji, odznacznie zmienionych własności w kolekcji(chyba)
        {
            return GetEnumerator();
        }
        public bool DelPerson(Person person) //odwołuje się VM przy usuwaniu osoby
        {
            return personsList.Remove(person);
        }
    }
}

