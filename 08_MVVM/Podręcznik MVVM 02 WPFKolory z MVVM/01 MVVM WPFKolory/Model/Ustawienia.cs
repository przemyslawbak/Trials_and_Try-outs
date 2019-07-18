using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_MVVM_WPFKolory.Model
{
    static class Ustawienia //warstwa dostepu do danych (s.31), część wartwy modelu
    {
        public static Kolor Czytaj()
        {
            Properties.Settings ustawienia = Properties.Settings.Default; //wczytywanie ustawień z default
            return new Kolor(ustawienia.R, ustawienia.G, ustawienia.B);

        }
        public static void Zapisz(Kolor kolor)
        {
            Properties.Settings ustawienia = Properties.Settings.Default; //zapisywanie usawien w default
            ustawienia.R = kolor.R;
            ustawienia.G = kolor.G;
            ustawienia.B = kolor.B;
            ustawienia.Save();
        }
    }
}
