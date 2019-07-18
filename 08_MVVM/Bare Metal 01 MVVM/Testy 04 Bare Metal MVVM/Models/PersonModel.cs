using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_04_Bare_Metal_MVVM.Models
{
    class PersonModel
    {
        private string name; //ciąg string imie

        public string Name //wlasnosc imie
        {
            get { return name; } //pobierz/zwróć
            set //zapisz...
            {
                if (name == value) //...jeśli ma wartość
                {
                    return;
                }
                name = value; //inaczej nadaj watrtość
            }
        }
    }
}
