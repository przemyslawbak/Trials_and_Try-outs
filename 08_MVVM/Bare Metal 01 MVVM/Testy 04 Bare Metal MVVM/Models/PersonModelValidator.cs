using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_04_Bare_Metal_MVVM.Models
{
    class PersonModelValidator
    {
        public bool IsValid(string name) //logiczna zmienna t/f dla ciągu string
        {
            if (!string.IsNullOrWhiteSpace(name)) //jeśłi nie jest null/space...
            {
                return true; //bool=true
            }
            else
            {
                Console.WriteLine("name cant be empty string"); //inaczej...
                return false;
            }
        }
    }
}
