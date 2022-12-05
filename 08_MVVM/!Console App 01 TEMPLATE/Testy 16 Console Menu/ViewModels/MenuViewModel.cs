using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Testy_16_Console_Menu.Models;

namespace Testy_16_Console_Menu.ViewModels
{
    class MenuViewModel
    {
        private readonly List<MenuItem> menuItems = new List<MenuItem>(); //instance of MenuItem collection
        private readonly DataModelValidator validator; //validates input data
        public MenuViewModel() //main menu
        {
            validator = new DataModelValidator();
            menuItems.Add(NewMenuItem("1. Modify entry", ModifyEntry));
            menuItems.Add(NewMenuItem("2. Insert entry", InsertEnrty));
            menuItems.Add(NewMenuItem("3. Delete entry", DeleteEntry));
            menuItems.Add(NewMenuItem("4. Exit application", ExitProgram));
        }
        private static MenuItem NewMenuItem(string header, Action commandAction) //method creating new menu item including command action
        {
            return new MenuItem
            {
                Header = header,
                Command = new RelayCommand(commandAction)
            };
        }
        public void PrintMenu(bool clearScreen) //printing new menu method
        {
            bool checkForDB = DatabaseRepository.CheckForDB();
            if (clearScreen) //if clearscreen
            {
                Console.Clear();
            }
            PrintHeader(); //print header
            if (!checkForDB)
            {
                DatabaseRepository.CreateDatabase(); //create database method. creates if not existing
            }
            else
            {
                DatabaseRepository.PopulateCheck(); //check if DB is populated
                DatabaseRepository.PrintAllData(); //print all from DB
            }
            Console.WriteLine($"Menu:{Environment.NewLine}");
            foreach (MenuItem menuItem in menuItems) //display all items from the list
            {
                Console.WriteLine(menuItem.Header);
            }
            Console.WriteLine("");
        }
        public void InputLoop() //loop for taking the input
        {
            while (true) //while still true
            {
                ConsoleKeyInfo key = Console.ReadKey(true); //get the key, true for hiding the cursor
                foreach (MenuItem menuItem in menuItems)
                {
                    if (menuItem.Header.StartsWith(key.KeyChar)) //if headers start matches the key
                    {
                        menuItem.Command.Execute(); //execute the command
                    }
                }
            }
        }
        //printing some extra stuff in console
        public void PrintHeader()
        {
            //https://devops.datenkollektiv.de/banner.txt/index.html
            Console.Title = "Console App 1.0";
            Console.WriteLine(@"  _____                        __             ___               
 / ___/ ___   ___   ___ ___   / / ___        / _ |   ___    ___ 
/ /__  / _ \ / _ \ (_-</ _ \ / / / -_)      / __ |  / _ \  / _ \
\___/  \___//_//_//___/\___//_/  \__/      /_/ |_| / .__/ / .__/
                                                  /_/    /_/ 

    v. 1.0 - by Przemyslaw Bak
");
        }
        public void ModifyEntry() //1. menu method
        {
            bool isIdValid;
            Console.WriteLine("Please enter entry`s ID to be modified:");
            string input = Console.ReadLine();
            isIdValid = validator.IsIdValid(input);
            if (isIdValid)
            {
                bool isNameValid, isDateValid, isSalaryValid;
                string inputName, inputSalary, inputDate = "";
                do
                {
                    inputName:
                    Console.WriteLine("Please enter model`s name:");
                    inputName = Console.ReadLine();
                    isNameValid = validator.IsNameValid(inputName);
                    if (!isNameValid) goto inputName;
                    inputSalary:
                    Console.WriteLine("Please enter model`s salary:");
                    inputSalary = Console.ReadLine();
                    isSalaryValid = validator.IsSalaryValid(inputSalary);
                    if (!isSalaryValid) goto inputSalary;
                    inputDate:
                    Console.WriteLine("Please enter model`s join date:");
                    inputDate = Console.ReadLine();
                    isDateValid = validator.IsDateValid(inputDate);
                    if (!isDateValid) goto inputDate;
                }
                while (!isNameValid || !isSalaryValid || !isDateValid);
                inputName = UppercaseFirst(inputName); //first letter up
                DatabaseModel edit = new DatabaseModel()
                {
                    ID = int.Parse(input),
                    Name = inputName,
                    Salary = double.Parse(inputSalary),
                    StartWork = DateTime.Parse(inputDate)
                };
                DatabaseRepository.EditItem(edit);
                PrintMenu(true);
                Console.WriteLine("Item added");
            }
            else
            {
                ModifyEntry();
            }
        }
        public void InsertEnrty() //2. menu method
        {
            bool isNameValid, isDateValid, isSalaryValid;
            string inputName, inputSalary, inputDate = "";
            do
            {
                inputName:
                Console.WriteLine("Please enter model`s name:");
                inputName = Console.ReadLine();
                isNameValid = validator.IsNameValid(inputName);
                if (!isNameValid) goto inputName;
                inputSalary:
                Console.WriteLine("Please enter model`s salary:");
                inputSalary = Console.ReadLine();
                isSalaryValid = validator.IsSalaryValid(inputSalary);
                if (!isSalaryValid) goto inputSalary;
                inputDate:
                Console.WriteLine("Please enter model`s join date:");
                inputDate = Console.ReadLine();
                isDateValid = validator.IsDateValid(inputDate);
                if (!isDateValid) goto inputDate;
            }
            while (!isNameValid || !isSalaryValid || !isDateValid);
            inputName = UppercaseFirst(inputName); //first letter up
            DatabaseModel input = new DatabaseModel()
            {
                Name = inputName,
                Salary = double.Parse(inputSalary),
                StartWork = DateTime.Parse(inputDate)
            };
            DatabaseRepository.AddItem(input);
            PrintMenu(true);
            Console.WriteLine("Item added");
        }
        public void DeleteEntry() //3. menu method
        {
            bool isIdValid;
            Console.WriteLine("Please enter entry`s ID to be removed:");
            string input = Console.ReadLine();
            PrintMenu(true);
            isIdValid = validator.IsIdValid(input);
            if (isIdValid)
            {
                DatabaseRepository.DeleteItem(input);
                PrintMenu(true);
                Console.WriteLine("Item deleted");
            }
            else
            {
                DeleteEntry();
            }
        }
        public void ExitProgram() //4. menu method
        {
            Environment.Exit(0);
        }
        string UppercaseFirst(string str) //making first letter of the name to upper case
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }
    }
}
