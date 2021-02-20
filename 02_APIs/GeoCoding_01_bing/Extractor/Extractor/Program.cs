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
                    string type = string.Empty;
                    string lat = string.Empty;
                    string lon = string.Empty;
                    counter++;
                    string line = reader.ReadLine();
                    string[] input = line.Split(_sep.ToCharArray());
                    string placeName = input[0];
                    string countryCode = input[1];
                    Console.Write(counter + " Finding for: Country-" + countryCode + " Name-" + placeName + " Result-");
                    string endpoint = @"http://dev.virtualearth.net/REST/v1/Locations?locality=" + placeName + "&countryRegion=" + countryCode + "&maxResults=10&key=<KEY>-";

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            using (HttpResponseMessage response = await client.GetAsync(endpoint))
                            using (HttpContent content = response.Content)
                            {

                                string result = await content.ReadAsStringAsync();

                                JObject joResponse = JObject.Parse(result);
                                type = joResponse.SelectToken("resourceSets[0].resources[0].entityType").ToString();
                                if (type == "PopulatedPlace")
                                {
                                    lat = joResponse.SelectToken("resourceSets[0].resources[0].point.coordinates[0]").ToString();
                                    lon = joResponse.SelectToken("resourceSets[0].resources[0].point.coordinates[1]").ToString();
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
                            Console.WriteLine("EXCEPTION-" + ex.Message);
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
