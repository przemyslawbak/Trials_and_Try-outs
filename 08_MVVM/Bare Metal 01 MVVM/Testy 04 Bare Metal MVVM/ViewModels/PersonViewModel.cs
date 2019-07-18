using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Testy_04_Bare_Metal_MVVM.Models;

namespace Testy_04_Bare_Metal_MVVM.ViewModels
{
    class PersonViewModel : INotifyPropertyChanged
    {
        private readonly PersonModel personModel; //zm
        private readonly PersonModelValidator validator; //zm



        public PersonViewModel() //konstr
        {
            personModel = new PersonModel(); //instancja modelu
            validator = new PersonModelValidator(); //instancja modelu
            Age = new AgeModel(); //instancja modelu

            //poniżej VM wykonuje komendy MenuItem, gdzie jest wołane GetMenuItem, wchodzimy nazwą(string) i akcją
            //własności klasy MenuItem Hearder i Command są odpowiednio przypisane
            Menu.Add(GetMenuItem("1. Change the name", InputName));
            Menu.Add(GetMenuItem("2. Change the foreground color to red", () => SetForegroundColor(ConsoleColor.DarkRed)));
            Menu.Add(GetMenuItem("3. Change the foreground color to green", () => SetForegroundColor(ConsoleColor.DarkGreen)));
            Menu.Add(GetMenuItem("4. Change the foreground color to white", () => SetForegroundColor(ConsoleColor.White)));
            Menu.Add(GetMenuItem("5. Change the background color to cyan", () => SetBackgroundColor(ConsoleColor.DarkCyan)));
            Menu.Add(GetMenuItem("6. Change the background color to yellow", () => SetBackgroundColor(ConsoleColor.DarkYellow)));
            Menu.Add(GetMenuItem("7. Change the background color to black", () => SetBackgroundColor(ConsoleColor.Black)));
            Menu.Add(GetMenuItem("q. Quit", () => Environment.Exit(0)));
        }


        private void SetForegroundColor(ConsoleColor consoleColor) //jako Command
        {
            Console.ForegroundColor = consoleColor;
            PrintMenu(true);
        }

        private void SetBackgroundColor(ConsoleColor consoleColor) //jako Command
        {
            Console.BackgroundColor = consoleColor;
            PrintMenu(true);
        }

        private static MenuItem GetMenuItem(string header, Action commandAction) //przekazujemy do klasy MenuItem Header i Command
        {
            return new MenuItem
            {
                Header = header, //przypisanie zmiennych własnościm klasy
                Command = new RelayCommand(commandAction) //przypisanie zmiennych własnościm klasy
            };
        }

        public Menu Menu { get; } = new Menu(); //instancja nowego Menu

        public void PrintMenu(bool clearScreen) //wyświetlenie menu. public, ponieważ będzie wołane z widoku
        {
            if (clearScreen) //jeśli true...
            {
                Console.Clear();
            }
            foreach (MenuItem menuItem in Menu.Items) //dla każdego item w menu
            {
                Console.WriteLine(menuItem.Header); //wyświetl Header
            }
        }

        public AgeModel Age { get; } //własność


        public string Name //własność
        {
            get { return personModel.Name; }
            set
            {
                if (!validator.IsValid(value))
                {
                    return;
                }
                personModel.Name = value;
                OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged; //event

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InputName() //co tu robi InputName?????????????????
        {
            Console.WriteLine($"{Environment.NewLine}Please enter a name");
            string input = Console.ReadLine();
            PrintMenu(true);
            Name = input;
        }


    }


}
