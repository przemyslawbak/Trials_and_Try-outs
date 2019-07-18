using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testy_04_Bare_Metal_MVVM.Models;

namespace Testy_04_Bare_Metal_MVVM.ViewModels
{
    public class Menu
    {
        private readonly List<MenuItem> menuItems = new List<MenuItem>(); //instancja kolekcji MenuList
        public void Add(MenuItem menuItem) //metoda dodania itemu do kolekcji
        {
            menuItems.Add(menuItem);
        }

        public IEnumerable<MenuItem>
            Items => menuItems;
    }
}
