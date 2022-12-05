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
            List<string> coordinates = await GetCoordinates();
            SaveToFile(coordinates);
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

        private static async Task<List<string>> GetCoordinates()
        {
            List<string> coordinates = new List<string>();
            using (var reader = new StreamReader("_input.txt"))
            {
                int counter = 0;
                while (!reader.EndOfStream)
                {
                    JToken token;
                    string type = string.Empty;
                    string lat = string.Empty;
                    string lon = string.Empty;
                    counter++;
                    string line = reader.ReadLine();
                    string[] input = line.Split(_sep.ToCharArray());
                    string placeName = input[0];
                    string countryCode = input[1];
                    Console.Write(counter + " Finding for: Country-" + countryCode + " Name-" + placeName + " Result-");
                    string endpoint = @"https://us1.locationiq.com/v1/search.php?key=" + "<KEY>" + placeName + "," + countryCode + @"&format=json&limit=1&tag=place:city,place:town,place:village";

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            using (HttpResponseMessage response = await client.GetAsync(endpoint))
                            using (HttpContent content = response.Content)
                            {

                                string result = await content.ReadAsStringAsync();
                                result.Substring(1, result.Length - 1);
                                JArray joArray = JArray.Parse(result);

                                var count = joArray.Count;

                                var xxx = joArray[0].SelectToken("place_id").ToString();
                                token = joArray[0].SelectToken("type");

                                if (token != null)
                                {
                                    type = joArray[0].SelectToken("type").ToString();

                                    if (type == "village" || type == "town" || type == "city" || type == "hamlet")
                                    {
                                        lat = joArray[0].SelectToken("lat").ToString();
                                        lon = joArray[0].SelectToken("lon").ToString();
                                    }
                                }

                                if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lat))
                                {
                                    Console.WriteLine("CORRECT");
                                }
                                else
                                {
                                    Console.WriteLine("WRONG");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("EXCEPTION");
                        }
                    }

                    string coordinate = lat + "|" + lon;
                    coordinates.Add(coordinate);
                }

                return coordinates;
            }
        }
    }
}
