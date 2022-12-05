using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<CityModel> cities = new List<CityModel>();
            List<AddressModel> addressesM = new List<AddressModel>();
            List<string> output = new List<string>();
            List<string> genericCities = File.ReadAllLines("1.txt").ToList();
            string[] addresses = File.ReadAllLines("2.txt");

            foreach (string line in genericCities)
            {
                cities.Add(new CityModel() { City = line.ToLower().Trim().Split(';')[0], CountryChar = line.ToLower().Trim().Split(';')[1][0] });
            }

            foreach (string line in addresses)
            {
                AddressModel model = new AddressModel();
                string[] items = line.ToLower().Trim().Split(';');

                if (items.Length == 2)
                {
                    if (items[1].Length > 0)
                    {
                        addressesM.Add(new AddressModel() { Address = items[0], CountryChar = items[1][0] });
                    }
                    else
                    {
                        addressesM.Add(new AddressModel() { Address = "1", CountryChar = 'x' });
                    }
                }
                else
                {
                    addressesM.Add(new AddressModel() { Address = "1", CountryChar = 'x' });
                }

            }

            int i = 0;

            foreach(AddressModel address in addressesM)
            {
                string city = "xxx";
                i++;

                Console.WriteLine(i + " z " + addresses.Count());

                if (address.Address.Contains(','))
                {
                    string[] array = address.Address.Split(',');
                    CityModel tocompare = cities.Where(c => array.Any(a => a.Trim() == c.City) && address.CountryChar == c.CountryChar).FirstOrDefault();
                    if (tocompare != null)
                    {
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                        tocompare.City = textInfo.ToTitleCase(tocompare.City);

                        city = tocompare.City;
                    }
                }

                output.Add(city);
            }

            File.WriteAllLines("output.txt", output);

            Console.WriteLine("saved");
            Console.ReadKey();

        }
    }
}
