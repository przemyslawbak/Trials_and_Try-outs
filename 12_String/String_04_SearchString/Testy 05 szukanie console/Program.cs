using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_05_szukanie_console
{
    class Program
    {


        static void Main(string[] args)
        {
            List<string> list = new List<string>();
            list.Add("involve");
            list.Add("this");
            list.Add("another");
            list.Add("direct");
            list.Add("finding");
            list.Add("returns");
            list.Add("means");
            list.Add("customers");
            list.Add("elements");
            list.Add("operation");
            list.Add("earlier");
            list.Add("dictionary");
            list.Add("identifies");
            list.Add("store");
            list.Add("specify");
            ObservableCollection<AdresViewModel> ListaAdresow = new ObservableCollection<AdresViewModel>();


            for (int i = list.Count - 1; i >= 0; i--)
            {
                AdresViewModel dodatek = new AdresViewModel(list[i], true, false);
                ListaAdresow.Add(dodatek);
                Console.WriteLine(dodatek.AdresMailowy);
            }

            //lista adresów zaludniona


            ObservableCollection<AdresViewModel> wyniki = new ObservableCollection<AdresViewModel>();
            string szukana;

            Console.WriteLine("wpisz szukaną:");
            szukana = Console.ReadLine();

            for (int i = ListaAdresow.Count - 1; i >= 0; i--)
            {
                if (ListaAdresow[i].AdresMailowy.Contains(szukana))
                {
                    wyniki.Add(ListaAdresow[i]);
                }
            }

            for (int i = wyniki.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(wyniki[i].AdresMailowy);
            }




            Console.ReadKey();
        }
    }



}
