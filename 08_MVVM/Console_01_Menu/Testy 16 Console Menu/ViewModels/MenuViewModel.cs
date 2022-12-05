using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testy_16_Console_Menu.Models;

namespace Testy_16_Console_Menu.ViewModels
{
    class MenuViewModel
    {
        private readonly List<MenuItem> menuItems = new List<MenuItem>(); //instance of MenuItem collection
        public MenuViewModel()
        {
            menuItems.Add(NewMenuItem("1. Print again menu", TestFunc1));
            menuItems.Add(NewMenuItem("2. Exit application", TestFunc2));
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
            if (clearScreen) //uf clearscreen
            {
                Console.Clear();
            }
            foreach (MenuItem menuItem in menuItems) //display all items from the list
            {
                Console.WriteLine(menuItem.Header);
            }
        }
        public void InputLoop() //loop for taking the input
        {
            while (true) //while still true
            {
                ConsoleKeyInfo key = Console.ReadKey(); //get the key
                foreach (MenuItem menuItem in menuItems)
                {
                    if (menuItem.Header.StartsWith(key.KeyChar)) //if headers start matches the key
                        menuItem.Command.Execute(); //execute the command
                }
            }
        }
        public void TestFunc1()
        {
            PrintMenu(true);
            Console.WriteLine("menu printed again");
        }
        public void TestFunc2()
        {
            Environment.Exit(0);
        }

    }
}
