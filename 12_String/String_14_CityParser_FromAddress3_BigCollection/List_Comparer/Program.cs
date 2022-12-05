using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<AddressModel> addressesM = new List<AddressModel>();
            List<string> output = new List<string>();
            List<string> genericCities = File.ReadAllLines("1.txt").ToList();
            string[] addresses = File.ReadAllLines("2.txt");
            Dictionary<(string city, char country), CityModel> citiesDict = new Dictionary<(string city, char country), CityModel>();

            foreach (string line in addresses)
            {
                AddressModel model = new AddressModel();
                string[] items = line.ToLower().Trim().Split(';');

                string addr = items[0];
                if (addr.Contains("-shi,"))
                {
                    addr = line.Replace("-shi,", ",");
                }

                if (items.Length == 2)
                {
                    if (items[1].Length > 0)
                    {
                        addressesM.Add(new AddressModel() { Address = addr, CountryChar = items[1][0] });
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

            foreach (string line in genericCities)
            {
                string cit = line.ToLower().Trim().Split(',')[1];
                char country = line.ToLower().Trim().Split(',')[0][0];

                CityModel city = new CityModel() { City = cit, CountryChar = country };

                if (!citiesDict.ContainsKey((cit, country)))
                {
                    citiesDict.Add((cit, country), city);
                }
            }

            var tocompare = addressesM
                    .SelectMany(item => CityNames(item.Address) // match all possible cities form address
                    .Select(possibleCity => new { // actual city from possible city
                        address = item,
                        city = citiesDict.TryGetValue((possibleCity, item.CountryChar),
                                          out var actualCity)
                                          ? actualCity // Either Real City (if found), say, "singapore"
                                          : null       // null if not exits, say, "road"
                    }))
                    .Where(item => item.city != null).ToList(); // Real City Only


            int i = 0;

            foreach (AddressModel address in addressesM)
            {
                string city = "xxx";
                i++;

                var c = tocompare.Where(toComp => address.Address == toComp.address.Address).FirstOrDefault();

                if (c != null)
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                    c.city.City = textInfo.ToTitleCase(c.city.City);

                    city = c.city.City;
                }

                Console.WriteLine(i + " z " + addresses.Count());

                output.Add(city);
            }

            File.WriteAllLines("output.txt", output);

            Console.WriteLine("saved");
            Console.ReadKey();

        }

        private static IEnumerable<string> CityNames(string address)
        {
            return address
      .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
      .Select(item => Regex.Replace(item.Trim(), @"\s+", " ").ToLower())
      .Where(item => !string.IsNullOrEmpty(item));
        }
    }
}
