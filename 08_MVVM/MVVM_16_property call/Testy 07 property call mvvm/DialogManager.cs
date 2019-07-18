using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Testy_07_property_call_mvvm
{
    public class Powiadomienie
    {
        public void ShowPowiadomienie(string wiadomosc, string caption)
        {

            MessageBox.Show(wiadomosc, caption);


        }
    }
}
