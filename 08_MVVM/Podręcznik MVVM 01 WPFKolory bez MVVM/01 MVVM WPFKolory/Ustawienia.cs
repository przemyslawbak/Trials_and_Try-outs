using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _01_MVVM_WPFKolory
{
    static class Ustawienia
    {
        public static Color Czytaj()
        {
            Properties.Settings ustawienia = Properties.Settings.Default; //wczytywanie ustawień z default
            Color kolor = new Color()
            {
                A = 255,
                R = ustawienia.R,
                G = ustawienia.G,
                B = ustawienia.B
            };
            return kolor;
        }
        public static void Zapisz(Color kolor)
        {
            Properties.Settings ustawienia = Properties.Settings.Default; //zapisywanie usawien w default
            ustawienia.R = kolor.R;
            ustawienia.G = kolor.G;
            ustawienia.B = kolor.B;
            ustawienia.Save();
        }
    }
}
