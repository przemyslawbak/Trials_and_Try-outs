using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Extractor
{
    class Program
    {
        private static readonly string _sep = "\t";

        static async Task Main(string[] args)
        {
            List<string> cities = await GetCities();
            SaveToFile(cities);
        }

        private static void SaveToFile(List<string> cities)
        {
            Console.WriteLine("Saving...");

            if (File.Exists("_result.txt"))
            {
                File.Delete("_result.txt");
            }

            System.IO.File.WriteAllLines("_result.txt", cities);
        }

        private static async Task<List<string>> GetCities()
        {
            List<string> cities = new List<string>();
            using (var reader = new StreamReader("_coordLines.txt"))
            {
                int counter = 0;
                while (!reader.EndOfStream)
                {
                    counter++;
                    string line = reader.ReadLine();
                    string[] coordinates = line.Split(_sep.ToCharArray());
                    string lat = coordinates[0].Replace(",", ".");
                    string lon = coordinates[1].Replace(",", ".");
                    Console.Write(counter + " Finding for: Lat-" + lat + " Lon-" + lon + " ");
                    string page = @"https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?f=json&langCode=EN&location=" + lat + "," + lon;

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            using (HttpResponseMessage response = await client.GetAsync(page))
                            using (HttpContent content = response.Content)
                            {
                                string result = await content.ReadAsStringAsync();

                                JObject joResponse = JObject.Parse(result);
                                JToken address = joResponse["address"];
                                string city = address.SelectToken("City").ToString();
                                string res = string.IsNullOrEmpty(city) ? "--------------------" : city;
                                Console.WriteLine("Result-" + res);
                                cities.Add(city);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception-" + ex.Message);
                        }
                    }
                }

                return cities;
            }
        }
    }
}
