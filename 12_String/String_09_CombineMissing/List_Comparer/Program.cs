using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] shipyardLines = File.ReadAllLines("1.txt");
            List<ShipyardModel> shipyards = new List<ShipyardModel>();

            foreach (string line in shipyardLines)
            {
                ShipyardModel s = new ShipyardModel()
                {
                    Imo = line.Split(';')[0],
                    ShipyardName = line.Split(';')[1],
                    ShipyardCity = line.Split(';')[2],
                    ShipyardCountry = line.Split(';')[3],
                };

                shipyards.Add(s);
            }

            for (int i = 0; i < shipyards.Count; i++)
            {
                Console.WriteLine("Przetwarzanie... " + i + " z " + shipyards.Count());

                if ((string.IsNullOrEmpty(shipyards[i].ShipyardCountry) || string.IsNullOrEmpty(shipyards[i].ShipyardCity)) && !string.IsNullOrEmpty(shipyards[i].ShipyardName))
                {
                    ShipyardModel completed = shipyards.Where(s => s.ShipyardName.Trim().ToLower() == shipyards[i].ShipyardName.Trim().ToLower() && !string.IsNullOrEmpty(s.ShipyardCountry) && !string.IsNullOrEmpty(s.ShipyardCity)).FirstOrDefault();

                    if (completed != null)
                    {
                        shipyards[i].ShipyardCountry = completed.ShipyardCountry;
                        shipyards[i].ShipyardCity = completed.ShipyardCity;
                    }
                    else
                    {
                        completed = shipyards.Where(s => s.ShipyardName.Trim().ToLower() == shipyards[i].ShipyardName.Trim() && !string.IsNullOrEmpty(s.ShipyardCountry)).FirstOrDefault();

                        if (completed != null)
                        {
                            shipyards[i].ShipyardCountry = completed.ShipyardCountry;
                        }

                    }
                }
            }

            int j = 0;

            foreach (ShipyardModel shipyard in shipyards)
            {
                j++;

                string line = CombineLineCompany(shipyard);
                Console.WriteLine("Zapisywanie stoczni... " + j + " z " + shipyards.Count);
                using (TextWriter csvLineBuilder = new StreamWriter("_output_shipyrds.txt", true))
                {
                    csvLineBuilder.WriteLine(line);
                }
            }

            Console.WriteLine("saved");
            Console.ReadKey();

        }

        private static string CombineLineCompany(ShipyardModel shipyard)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(shipyard.Imo);
            sb.Append("|");
            sb.Append(shipyard.ShipyardName);
            sb.Append("|");
            sb.Append(shipyard.ShipyardCity);
            sb.Append("|");
            sb.Append(shipyard.ShipyardCountry);

            return sb.ToString();
        }
    }
}
