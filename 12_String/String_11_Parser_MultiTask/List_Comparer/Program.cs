using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace List_Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            Stuff().Wait();

            Console.ReadKey();
        }

        private static async Task Stuff()
        {
            int _degreeOfParallelism = 100;
            List<string> cityBase = new List<string>();
            List<string> output = new List<string>();
            List<string> genericCities = File.ReadAllLines("worldcitiespop.csv").ToList();
            string[] addresses = File.ReadAllLines("2.txt");

            int j = 0;
            foreach (string line in genericCities)
            {
                j++;
                Console.WriteLine(j + " z " + genericCities.Count());
                cityBase.Add(line.Split(',')[1]);
            }

            List<Task> tasks = new List<Task>();
            SemaphoreSlim throttler = new SemaphoreSlim(_degreeOfParallelism);

            for (int i = 0; i < addresses.Count(); i++)
            {
                int iteration = i;
                tasks.Add(Task.Run(async () =>
                {
                    await throttler.WaitAsync();
                    try
                    {
                        string tocompare = genericCities.Where(c => c.ToLower().Trim() == addresses[iteration].ToLower().Trim()).FirstOrDefault();

                        lock (((ICollection)output).SyncRoot)
                        {
                            if (!string.IsNullOrEmpty(tocompare))
                            {
                                output.Add(tocompare);
                            }
                            else
                            {
                                output.Add("xxx");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        throttler.Release();
                        Console.WriteLine(iteration + " z " + addresses.Count());
                    }

                }));
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                File.WriteAllLines("output.txt", output);
                Console.WriteLine("saved");
            }
        }
    }
}
