using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Testy_06_XmlLoad_console
{
    public class ModelOsoby
    {
        public string OsobaImie
        {
            get;
            set;
        }

        public string OsobaNazwisko
        {
            get;
            set;
        }

        public ModelOsoby(string osobaImie, string osobaNazwisko)
        {
            OsobaImie = osobaImie;
            OsobaNazwisko = osobaNazwisko;
        }

        public static void ZapisDoPliku(string imie, string nazwisko)
        {
            XDocument document = new XDocument();
            document.Add(new XElement("Ustawienia",
                            new XElement("Imie", imie),
                            new XElement("Nazwisko", nazwisko)));
            document.Save("zapis.xml");
        }

        public static void WczytajZPliku(string filePath)
        {
            ModelOsoby person = new ModelOsoby("", "");
            XDocument document = XDocument.Load(filePath);
            foreach (XElement element in document.Descendants("Ustawienia"))
            {
                Console.WriteLine(element.Element("Imie").Value);
                Console.WriteLine(element.Element("Nazwisko").Value);
            }
        }
    }



    class Program
    {



        static void Main(string[] args)
        {
            string sciezka = "zapis.xml";
            ModelOsoby person = new ModelOsoby("", "");
            Console.WriteLine("podaj imię:");
            person.OsobaImie = Console.ReadLine();
            Console.WriteLine("podaj nazwisko:");
            person.OsobaNazwisko = Console.ReadLine();
            ModelOsoby.ZapisDoPliku(person.OsobaImie, person.OsobaNazwisko);
            Console.WriteLine("Wciśnij dowolny przycisk...");
            Console.ReadKey();
            Console.WriteLine("Odczyt z pliku:");
            ModelOsoby.WczytajZPliku(sciezka);
            Console.WriteLine("Wciśnij dowolny przycisk...");
            Console.ReadKey();




        }


    }
}

