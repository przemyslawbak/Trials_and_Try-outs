using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Podręcznik_MVVM_04_ZadaniaWPF.Model
{
    //eksport zadań do pliku TXT
    public static class PlikTXT //klasa
    {
        public static void Zapisz(string ścieżkaPliku, Zadania zadania) //do metody odwołuje się klasa Zadania interfejs Zapisz
        {
            if (!string.IsNullOrWhiteSpace(ścieżkaPliku)) //jeśli ścieżka pliku nie jest równa zero, to...
            {
                List<string> opisyZadań = new List<string>(); //utworzenie instancji listy
                foreach (Zadanie zadanie in zadania) opisyZadań.Add(zadanie.ToString());
                System.IO.File.WriteAllLines(ścieżkaPliku, opisyZadań.ToArray());
            }
        }
    }


}
