using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testy_04_Bare_Metal_MVVM.Models;

namespace Testy_04_Bare_Metal_MVVM.Views
{
    public abstract class Framework
    {
        private readonly Dictionary<string, Binding> bindings = new Dictionary<string, Binding>(); //instancja słownika łączeń, słownik kluczy własności

        public object DataContext { get; set; } //własność/field

        protected void SetBinding(string property) //metoda
        {
            Binding binding = new Binding(property) //instancja bindowania własności
            {
                Source = DataContext //przypisanie VM do źródła (bindowania?)

            };

            binding.Parse(); //sprawdzamy z metodą Parse czy jest podłączony INPC. jeśłi tak, podłączamy PropertyChanged event handler
            bindings.Add(property, binding);  //nowe wiązanie, dodanie do słownika
        }

        protected virtual void Initialize() //metoda, odpalenie! jakiejś pętli chyba, ale nie wiem
        {
            InputLoop();
        }


        /// <summary>
        /// This is just iterating over all the none null menu entries in the bindings,
        /// and then looking for entries where the first character in the menu header matches the key the user pressed.
        /// When it finds a match, it's going to execute the command associated with that menu item.
        /// </summary>
        private void InputLoop() //pętla w menu, metoda
        {
            while (true) //gdy prawda (skąd?)
            {
                ConsoleKeyInfo key = Console.ReadKey(); //przypisanie guzika do zmiennej
                //dla każdego elementu MenuItem w wartościach słownika kluczy, gdie bindujemy Menu...
                foreach (MenuItem menuItem in bindings.Values.Where(binding => binding.Menu != null)
                    .SelectMany(binding => binding.Menu.Items.Where(//...wybierz elementy które nie są null
                    menuItem => menuItem.Header.Substring(0, 1) == key.KeyChar.ToString()))) //porównuje wciśnięty guzik z pierwszym znakiem headera
                {
                    menuItem.Command.Execute();//jeśłi wciśnięty przysk się zgadza, wykonujemy przypisaną komendę z klasy MenuItem
                }
            }
        }
    }
}
