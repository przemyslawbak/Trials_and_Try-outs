using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Testy_04_Bare_Metal_MVVM.ViewModels;

namespace Testy_04_Bare_Metal_MVVM.Views
{
    public class Binding
    {
        public object Source { get; set; } //własność/pole dla Parse
        public string Name { get; set; } //własność/pole dla Binding

        public Menu Menu { get; private set; } //provide a Menu property that exposes our Menu 
        public Binding(string property) //konstr, wołany przez Framework
        {
            Name = property;
        }

        //HashSet to zestaw wartości, gdzie T to typ
        private static readonly HashSet<object> instances = new HashSet<object>(); //Zestaw wartości obiektów, instancja

        //wyjście na konsolę
        public void Parse() //wołane przez Framework
        {//eee do końca nie wiem jeszcze czemu wchodzimy dwiema. chyba jedna się staje typem własności. do prześledzenia
            object value = Show(Source, Name); //sprawdza nazwę
            Menu menu = value as Menu; //method to check the entry so that, when the value of the bind is a Menu, it adds to our new property
            if (menu != null)
            {
                Menu = menu; //dodajemy Menu do własności Menu
                return;
            }

            Console.WriteLine($"{Name} is {value}"); //drukuje nazwę
            INotifyPropertyChanged inpc = Source as INotifyPropertyChanged; //i znów walidacja na INPC
            if (inpc == null) //jeśli nie ma zmiany, nic się nie dzieje
            {
                return;
            }
            if (instances.Contains(inpc)) //jeśli zmiana jest tylko w obiektach, nic się nie dzieje
            {
                return;
            }
            else
            {
                instances.Add(inpc);// w innym wypadku dodaj zmianę
                inpc.PropertyChanged += Inpc_PropertyChanged; //odpal event
            }
        }

        //zwraca Nazwę
        private static object Show(object type, string propertyName) //wchodzimy Source i Name, taka trochę walidacja dla errorów
        {
            while (true) //gdy prawda... (gdy jest wywołane Show przez Parse)
            {
                string[] properties = propertyName.Split('.'); //zwraca własności, które zawierają kropkę
                PropertyInfo propertyInfo; //przypisanie zmiennej klasy PropertyInfo
                if (properties.Length > 1) //jeśli ilość zmiennych jest większa niż 1 ...
                {
                    string property = properties.First(); //...to zmienna property jest pierwszą z nich
                    propertyName = propertyName.Substring(property.Length + 1); //szukamy nazwy własności drugiej
                    propertyInfo = type.GetType().GetProperty(propertyName); //...szukamy własności danego typu dla nazwy własności...
                    if (propertyInfo == null) //...jesli nie ma propertyInfo dla wyszukanej własności...
                    {
                        return $"Data binding error: Unable to get {propertyName} from {type.GetType().FullName}"; //to zwracamy komunikat błędu
                    }
                    type = propertyInfo.GetValue(type);
                    continue;
                }
                else //gdy ilość zmiennych mniejsza równa 1
                {
                    propertyInfo = type.GetType().GetProperty(propertyName);
                    if (propertyInfo == null)//...jesli nie ma propertyInfo dla wyszukanej własności...
                    {
                        return $"Data binding error: Unable to get {propertyName} from {type.GetType().FullName}";//to zwracamy komunikat błędu
                    }
                    object value = propertyInfo.GetValue(type);
                    return value ?? "<<Unset>>";
                }
            }
        }


        private void Inpc_PropertyChanged(object sender, PropertyChangedEventArgs e) //event dla zmiany w stringu
        {
            PropertyInfo propertyInfo = Source.GetType().GetProperty(e.PropertyName);
            object value = propertyInfo?.GetValue(Source);
            Console.WriteLine($"{e.PropertyName} changed to {value}");
        }


    }
}
